using Bidesk.Application.DTOs.User;
using Bidesk.Domain.Entities.Public;

namespace Bidesk.Application.Abstraction.Services.User
{
    public interface IUserService
    {
        Task<string[]> GetRolesToUserAsync(string userId);
        Task<bool> HasRolePermissionToEndpointAsync(string name, string code);
        //public AppUserEnt? GetUserByUserID(long userID);
        Task<AppUserEnt?> GetUserByUserIDAsync(long userID);
        //public AppUserEnt? GetCurrentUser();
        Task<AppUserEnt?> GetCurrentUserAsync();
        //public AppUserEnt? GetUserByEmail(string email);
        public Task<AppUserEnt?> GetUserByUserNameAsync(string userName);
        //public AppUserEnt? GetUserByRegistrationNo(string registrationNo);
        public Task<bool> IsUserExistAsync(long id);
        //public List<AppUserEnt> GetUserByIDRange(List<long> userIDs);
        public Task<List<AppUserEnt>> GetUserByIDRangeAsync(List<long> userIDs);
        public Task<List<UserDto>> GetAllUsersAsync(int page, int size);
        public Task<List<UserDto>> GetAllActiveUsersAsync(int page, int size);

        /// <summary>
        /// Personel listesi: isten ayrilmamis kullanicilar, departman adiyla birlikte.
        /// GetAllActiveUsersAsync departmani Include etmedigi icin ayri tutulur.
        /// </summary>
        public Task<List<PersonnelDto>> GetActivePersonnelAsync();
        /// <summary>
        /// Kullanici listesi onbellegi icin ucuz damga: kayit sayisi + en buyuk ID + ayrilan
        /// sayisi. Kullanicilar Portal API tarafinda yazildigi icin Bidesk tarafinda
        /// write-through invalidation kurulamiyor; bayatligi bu damga yakalar.
        /// AspNetUsers'ta guncelleme tarihi kolonu olmadigindan ekleme / silme / isten
        /// ayrilma yakalanir, mevcut kaydin ad degisikligi TTL doluncaya kadar yakalanmaz.
        /// </summary>
        public Task<string> GetUsersCacheStampAsync();

        public long GetCurrentUserID();
    }
}
