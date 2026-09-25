using Bidesk.Application.Abstraction.Services.User;
using Bidesk.Application.Custom_Attributes;
using Bidesk.Domain.Entities.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Bidesk.API.Filters
{
    public class RolePermissionFilter : IAsyncActionFilter
    {
        readonly IUserService _userService;
        readonly ILogger<RolePermissionFilter> _logger;

        public RolePermissionFilter(IUserService userService, ILogger<RolePermissionFilter> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <remarks>
        /// BU FILTRE IAsyncActionFilter OLARAK KALMALIDIR, IAsyncAuthorizationFilter'a CEVRILMEMELIDIR.
        /// Program.cs'te varsayilan sema JwtBearerDefaults.AuthenticationScheme, JWT handler'i ise
        /// "Loggedin" adiyla kayitli. Dolayisiyla HttpContext.User'i authentication middleware degil,
        /// action uzerindeki [Authorize(AuthenticationSchemes = "Loggedin")] dolduruyor. Global bir
        /// authorization filter o attribute'tan ONCE calisacagi icin kimligi bos gorur ve her istege
        /// 401 doner.
        /// </remarks>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Filtre bastan sona "varsayilan olarak reddet" mantigiyla calisir:
            //izin verilmesi icin her adimin acikca gecilmesi gerekir, atlanan adim erisim demek degildir.

            if (context.ActionDescriptor is not ControllerActionDescriptor descriptor)
            {
                _logger.LogError("Action descriptor cozulemedi, erisim reddedildi. Endpoint: {Endpoint}",
                    context.ActionDescriptor.DisplayName);
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                return;
            }

            //Anonim erisim YALNIZCA framework'un kendi [AllowAnonymous] attribute'u ile acilir;
            //endpoint metadata'si kullanildigi icin controller ve action seviyesi birlikte kapsanir.
            if (context.HttpContext.GetEndpoint()?.Metadata.GetMetadata<IAllowAnonymous>() != null)
            {
                await next();
                return;
            }

            //Kimlik dogrulanmadan HICBIR denetim atlanmaz. Identity?.Name tek basina yeterli degil;
            //IsAuthenticated de aranir.
            System.Security.Principal.IIdentity? identity = context.HttpContext.User.Identity;
            string? name = identity?.Name;

            if (identity?.IsAuthenticated != true || string.IsNullOrWhiteSpace(name))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            AppUserEnt? appUserEnt = await _userService.GetUserByUserNameAsync(name);

            if (appUserEnt == null)
            {
                //Token gecerli ama kullanici karsiligi yok: silinmis/yeniden adlandirilmis hesap olabilir.
                _logger.LogWarning("Token'daki kullanici adi veritabaninda bulunamadi: {UserName}", name);
                context.Result = new UnauthorizedResult();
                return;
            }

            if (appUserEnt.IsAdmin)
            {
                await next();
                return;
            }

            //[AllowBypassFilter] KIMLIK DENETIMINDEN SONRA degerlendirilir: anlami "dogrulanmis
            //kullanici, izin kodu aranmasin"dir, "herkese acik" DEGILDIR. Anonim erisim isteniyorsa
            //[AllowAnonymous] kullanilmalidir (yukarida). Yalnizca metot seviyesi okunur; controller
            //seviyesine yazilan bir bypass sessizce yok sayilir, yani guvenli tarafa duser.
            if (descriptor.MethodInfo.GetCustomAttributes<AllowBypassFilterAttribute>(inherit: true).Any())
            {
                await next();
                return;
            }

            //Birden fazla attribute bulunmasi GetCustomAttribute'ta AmbiguousMatchException'a yol acardi.
            AuthorizeDefinitionAttribute? attribute = descriptor.MethodInfo
                .GetCustomAttributes<AuthorizeDefinitionAttribute>(inherit: true)
                .FirstOrDefault();

            if (attribute == null || string.IsNullOrWhiteSpace(attribute.Definition))
            {
                //Izin kodu uretilemiyorsa erisim verilemez; eksik tanim gelistirme hatasidir, aciga donusmemelidir.
                _logger.LogError("AuthorizeDefinition eksik veya Definition bos, erisim reddedildi. Endpoint: {Endpoint}",
                    descriptor.DisplayName);
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                return;
            }

            //HTTP metodu attribute'tan okunur; yoksa sabit GET yerine istegin gercek metodu kullanilir.
            string httpMethod = descriptor.MethodInfo
                .GetCustomAttributes<HttpMethodAttribute>(inherit: true)
                .SelectMany(a => a.HttpMethods)
                .FirstOrDefault() ?? context.HttpContext.Request.Method;

            string code = $"{attribute.Portal}.{httpMethod}.{attribute.ActionType}.{attribute.Definition.Replace(" ", string.Empty)}";

            bool hasRole;
            try
            {
                hasRole = await _userService.HasRolePermissionToEndpointAsync(appUserEnt.Id.ToString(), code);
            }
            catch (Exception ex)
            {
                //Istek ilerlemez. Exception YENIDEN FIRLATILMAZ: global hata yakalayici
                //ex.Message'i yanit govdesine yaziyor ve istemci onu kullaniciya gosteriyor
                //(ornegin "Cannot create a DbSet for 'IdentityUserRole<long>'"). Ic detaylar
                //yalnizca sunucu logunda kalir; istemciye govdesiz 500 doner ve kullanici
                //arayuzun genel mesajini gorur.
                _logger.LogError(ex, "Izin sorgusu basarisiz. Kullanici: {UserId}, Kod: {Code}", appUserEnt.Id, code);
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                return;
            }

            if (!hasRole)
            {
                //Yanit govdesi bilerek bostur: izin kodu istemciye sizdirilmaz, yalnizca loglanir.
                //Istemci 403'u zaten "Bu islem icin yetkiniz bulunmamaktadir!" mesajiyla gosteriyor.
                _logger.LogWarning("Yetki reddedildi. Kullanici: {UserName} ({UserId}), Kod: {Code}",
                    name, appUserEnt.Id, code);
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                return;
            }

            await next();
        }
    }
}
