using Bidesk.Application.Abstraction.Services;
using Bidesk.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Bidesk.Persistence.Services
{
    public class ContextService : IContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Arka plan (Hangfire) işlerinde HttpContext yoktur; bu durumda varsayılan şirket/kullanıcı
        // döndürülür ki CompanyId-scope'lu repository'ler ve BaseContext.SaveChangesAsync çalışabilsin.
        // (Suggestion.ContextService ile aynı yaklaşım — DBPortal veritabanı iki modülde ortak.)
        private const long DefaultJobCompanyId = 1;
        private const long DefaultJobUserId = 1758;

        public long GetCurrentCompanyId()
        {
            if (_httpContextAccessor == null) return DefaultJobCompanyId;
            if (_httpContextAccessor.HttpContext == null) return DefaultJobCompanyId;

            Claim? company = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Company");

            if (company == null) return DefaultJobCompanyId;

            if (long.TryParse(company.Value, out long companyId))
                return companyId;
            else
                throw new MissingInfoException("Şirket id'si alınamadı!");
        }

        public string GetCurrentUserEmail()
        {
            if (_httpContextAccessor == null) throw new MissingInfoException("Context bilgisi alınamadı!");

            Claim? email = _httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Email");

            if (email == null) throw new MissingInfoException("Kullanıcı id'si alınamadı!");

            return email.Value;
        }

        public long GetCurrentUserId()
        {
            if (_httpContextAccessor == null) return DefaultJobUserId;
            if (_httpContextAccessor.HttpContext == null) return DefaultJobUserId;

            Claim? user = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");

            if (user == null) return DefaultJobUserId;

            if (long.TryParse(user.Value, out long userId))
                return userId;
            else
                throw new MissingInfoException("Kullanıcı id'si alınamadı!");
        }

        public string GetCurrentUserName()
        {
            if (_httpContextAccessor == null) throw new MissingInfoException("Context bilgisi alınamadı!");
            if (_httpContextAccessor.HttpContext == null) throw new MissingInfoException("Context bilgisi alınamadı!");
            if (_httpContextAccessor.HttpContext.User.Identity == null) throw new MissingInfoException("Kullanıcı adı alınamadı!");

            return _httpContextAccessor.HttpContext.User.Identity.Name!;
        }

        /// <summary>
        /// Taklit oturumunda gercek aktor (yonetici). Claim yalnizca Portal'in taklit
        /// token'inda bulunur; normal oturumlarda null doner.
        /// </summary>
        public long? GetImpersonatedByUserId() => ReadLongClaim("ImpersonatedBy");

        public long? GetImpersonationSessionId() => ReadLongClaim("ImpersonationSessionId");

        public bool IsWriteAllowed()
        {
            // Taklit oturumu degilse yazma serbesttir; kisit yalnizca taklide ozgudur.
            if (GetImpersonatedByUserId() == null) return true;

            Claim? write = _httpContextAccessor?.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "ImpersonationWrite");

            return write != null && bool.TryParse(write.Value, out bool allowed) && allowed;
        }

        private long? ReadLongClaim(string claimType)
        {
            Claim? claim = _httpContextAccessor?.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == claimType);

            if (claim == null) return null;

            return long.TryParse(claim.Value, out long value) ? value : null;
        }

        public bool IsAdmin()
        {
            if (_httpContextAccessor == null) throw new MissingInfoException("Context bilgisi alınamadı!");
            if (_httpContextAccessor.HttpContext == null) throw new MissingInfoException("Context bilgisi alınamadı!");

            Claim? admin = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "IsAdmin");

            if (admin == null) return false;

            if (bool.TryParse(admin.Value, out bool isAdmin))
                return isAdmin;
            else
                return false;
        }
    }
}
