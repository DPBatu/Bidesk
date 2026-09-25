using Bidesk.Application.Abstraction.Services;
using Bidesk.Application.Repositories.Public.ImpersonationActionLog;
using Bidesk.Domain.Entities.Public;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Bidesk.API.Filters
{
    /// <summary>
    /// Kullanıcı taklidi (impersonation) oturumları için yazma denetimi ve işlem izi.
    ///
    /// Taklit token'ı Portal tarafından üretilir ama TÜM portallarda geçerlidir; bu yüzden
    /// aynı denetim Kaizen API'sinde de uygulanmalıdır. Aksi halde salt okunur bir taklit
    /// oturumu, Kaizen uçlarından yazmaya devam edebilirdi.
    ///
    /// İşlem kaydı Portal'ın merkezi tablosuna (public.ImpersonationActionLogs) yazılır;
    /// böylece "bu kaizeni aslında hangi yönetici değiştirdi" sorusu tek yerden yanıtlanır.
    /// Normal oturumlar bu filtreden etkilenmez.
    /// </summary>
    public class ImpersonationGuardFilter : IAsyncActionFilter
    {
        private static readonly string[] MutatingMethods = { "POST", "PUT", "PATCH", "DELETE" };

        readonly IContextService _contextService;
        readonly IImpersonationActionLogWriteRepository _actionLogWriteRepository;

        public ImpersonationGuardFilter(
            IContextService contextService,
            IImpersonationActionLogWriteRepository actionLogWriteRepository)
        {
            _contextService = contextService;
            _actionLogWriteRepository = actionLogWriteRepository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            long? adminUserId = SafeGetImpersonatedBy();

            if (adminUserId == null)
            {
                await next();
                return;
            }

            string method = context.HttpContext.Request.Method.ToUpperInvariant();
            string path = context.HttpContext.Request.Path.Value ?? string.Empty;
            bool isMutating = MutatingMethods.Contains(method);

            if (isMutating && !_contextService.IsWriteAllowed())
            {
                context.Result = new ObjectResult(new
                {
                    isSuccess = false,
                    message = "Taklit oturumu",
                    errorMessage = "Taklit oturumu salt okunurdur; bu işlem yapılamaz."
                })
                { StatusCode = StatusCodes.Status403Forbidden };
                return;
            }

            ActionExecutedContext executed = await next();

            if (!isMutating) return;

            await TryLogAsync(adminUserId.Value, method, path, executed.HttpContext.Response.StatusCode);
        }

        /// <summary>Denetim kaydı tutulamadı diye kullanıcının işlemi hata almamalı.</summary>
        private async Task TryLogAsync(long adminUserId, string method, string path, int statusCode)
        {
            try
            {
                long? sessionId = _contextService.GetImpersonationSessionId();
                if (sessionId == null) return;

                await _actionLogWriteRepository.AddAsync(new ImpersonationActionLogEnt
                {
                    SessionID = sessionId.Value,
                    AdminUserID = adminUserId,
                    TargetUserID = _contextService.GetCurrentUserId(),
                    HttpMethod = method,
                    // Sorgu dizesi bilerek alınmaz — kişisel veri log'a sızmasın.
                    Path = path.Length > 400 ? path.Substring(0, 400) : path,
                    Source = "kaizen",
                    StatusCode = statusCode,
                    OccurredAt = DateTime.Now
                });

                await _actionLogWriteRepository.SaveAsync();
            }
            catch (Exception) { }
        }

        private long? SafeGetImpersonatedBy()
        {
            try { return _contextService.GetImpersonatedByUserId(); }
            catch (Exception) { return null; }
        }
    }
}
