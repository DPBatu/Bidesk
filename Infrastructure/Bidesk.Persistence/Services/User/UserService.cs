using Bidesk.Application.Abstraction.Services;
using Bidesk.Application.Abstraction.Services.User;
using Bidesk.Application.DTOs.User;
using Bidesk.Application.Exceptions;
using Bidesk.Application.Repositories.Public.Endpoint;
using Bidesk.Application.Repositories.Public.RoleRelation;
using Bidesk.Domain.Entities.Public;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Data;

namespace Bidesk.Persistence.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUserEnt> _userManager;
        private readonly RoleManager<AppRoleEnt> _roleManager;
        private readonly IContextService _contextService;
        private readonly IEndpointReadRepository _endpointReadRepository;
        private readonly IRoleRelationReadRepository _relationReadRepository;
        private readonly IMemoryCache _memoryCache;

        //İzin sonuçları bu süre kadar cache'lenir; rol/izin değişiklikleri en geç bu süre sonunda etkili olur.
        private static readonly TimeSpan PermissionCacheDuration = TimeSpan.FromMinutes(5);

        public UserService(
            UserManager<AppUserEnt> userManager,
            RoleManager<AppRoleEnt> roleManager,
            IContextService contextService,
            IEndpointReadRepository endpointReadRepository,
            IRoleRelationReadRepository relationReadRepository,
            IMemoryCache memoryCache)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _contextService = contextService;
            _endpointReadRepository = endpointReadRepository;
            _relationReadRepository = relationReadRepository;
            _memoryCache = memoryCache;
        }

        public async Task<string[]> GetRolesToUserAsync(string userId)
        {
            AppUserEnt? user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı!");

            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles.ToArray();
        }

        private async Task<IList<string>> GetChildRolesAsync(string roleName)
        {
            HashSet<string> visited = new HashSet<string>();
            await CollectChildRolesAsync(roleName, visited);
            return visited.ToList();
        }

        private async Task CollectChildRolesAsync(string roleName, HashSet<string> visited)
        {
            //Döngüsel rol ilişkilerinde (A->B->A) sonsuz özyinelemeyi engeller.
            if (!visited.Add(roleName))
                return;

            var directChildRoles = await _relationReadRepository.GetAll()
                .Include(r => r.Parent)
                .Include(r => r.Child)
                .Where(r => r.Parent.Name == roleName)
                .Select(r => r.Child.Name!)
                .ToListAsync();

            foreach (var childRole in directChildRoles)
            {
                await CollectChildRolesAsync(childRole, visited);
            }
        }

        public async Task<bool> HasRolePermissionToEndpointAsync(string id, string code)
        {
            string cacheKey = $"RolePermission:{id}:{code}";
            if (_memoryCache.TryGetValue(cacheKey, out bool cachedResult))
                return cachedResult;

            AppUserEnt? user = await _userManager.FindByIdAsync(id);

            if (user == null)
                throw new BaseException("Kullanıcı bulunamadı!");

            IList<string> userRoles = await _userManager.GetRolesAsync(user);
            HashSet<string> childRoles = new HashSet<string>();

            for (int i = 0; i < userRoles.Count; i++)
            {
                await CollectChildRolesAsync(userRoles[i], childRoles);
            }

            AppRoleEnt? role = await _roleManager.Roles.Where(r => childRoles.Contains(r.Name))
                .Include(r => r.Endpoints)
                .FirstOrDefaultAsync(r => r.Endpoints.Any(e => e.Code == code));

            bool hasPermission = role != null;
            _memoryCache.Set(cacheKey, hasPermission, PermissionCacheDuration);

            return hasPermission;
        }


        //public AppUserEnt? GetUserByUserID(long userID)
        //    => _userManager.Users.Where(u => u.Id == userID).FirstOrDefault();

        public async Task<AppUserEnt?> GetUserByUserIDAsync(long userID)
            => await _userManager.Users
                .Where(u => u.Id == userID)
                .FirstOrDefaultAsync();

        public async Task<AppUserEnt?> GetUserByUserNameAsync(string userName)
            => await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);

        //public AppUserEnt? GetUserByRegistrationNo(string registrationNo)
        //    => _userManager.Users.Where(u => u.RegistrationNo == registrationNo).FirstOrDefault();

        //public AppUserEnt? GetUserByEmail(string email)
        //    => _userManager.Users.Where(u => u.Email == email).FirstOrDefault();

        public async Task<bool> IsUserExistAsync(long id)
            => await _userManager.Users.AnyAsync(u => u.Id == id);

        //public List<AppUserEnt> GetUserByIDRange(List<long> userIDs)
        //  => _userManager.Users.Where(u => userIDs.Contains(u.Id)).ToList();

        public async Task<List<AppUserEnt>> GetUserByIDRangeAsync(List<long> userIDs)
          => await _userManager.Users.Where(u => userIDs.Contains(u.Id)).ToListAsync();

        public AppUserEnt? GetCurrentUser()
            => _userManager.Users.FirstOrDefault(u => u.Id == _contextService.GetCurrentUserId());

        public async Task<AppUserEnt?> GetCurrentUserAsync()
            => await _userManager.Users.FirstOrDefaultAsync(u => u.Id == _contextService.GetCurrentUserId());

        public async Task<string> GetUsersCacheStampAsync()
        {
            var stamp = await _userManager.Users
                .GroupBy(x => 1)
                .Select(g => new
                {
                    Count = g.Count(),
                    MaxID = g.Max(x => x.Id),
                    DismissedCount = g.Count(x => x.DismissalDate != null)
                })
                .FirstOrDefaultAsync();

            return stamp == null ? "0" : $"{stamp.Count}-{stamp.MaxID}-{stamp.DismissedCount}";
        }

        public async Task<List<UserDto>> GetAllUsersAsync(int page, int size)
        {
            List<AppUserEnt> users;
            if (page == 0 && size == 0)
            {
                users = await _userManager.Users.ToListAsync();
            }
            else
            {
                users = await _userManager.Users
                                .Skip(page * size)
                                .Take(size)
                                .ToListAsync();
            }

            return users.Select(user => new UserDto
            {
                ID = user.Id,
                RegistrationNo = user.RegistrationNo,
                UserName = user.UserName,
                Name = user.Name,
                Surname = user.Surname,
                NameSurname = user.NameSurname,
                PhoneNumber = user.PhoneNumber,
                CompanyPhoneNumber = user.CompanyPhoneNumber,
                IntercomNumber = user.IntercomNumber,
                SAPUserName = user.SAPUserName,
                ComputerName = user.ComputerName,
                EmployeeClassification = user.EmployeeClassification,
                HireDate = user.HireDate,
                DismissalDate = user.DismissalDate,
                Gender = user.Gender,
                MaritalStatus = user.MaritalStatus,
                BirthDate = user.BirthDate,
                CardNumber = user.CardNumber,
                Department = user.Department,
                DepartmentLogs = user.DepartmentLogs,
                Position = user.Position,
                PositionLogs = user.PositionLogs,
                IsAdmin = user.IsAdmin,
                ResponsibleDepartment = user.ResponsibleDepartment
            }).ToList();
        }

        public async Task<List<PersonnelDto>> GetActivePersonnelAsync()
        {
            return await _userManager.Users
                .Include(u => u.Department)
                .Where(u => u.DismissalDate == null || u.DismissalDate > DateTime.Now)
                .Select(u => new PersonnelDto
                {
                    ID = u.Id,
                    NameSurname = u.NameSurname,
                    RegistrationNo = u.RegistrationNo,
                    DepartmentName = u.Department != null ? u.Department.Description : null
                })
                .OrderBy(u => u.NameSurname)
                .ToListAsync();
        }

        public async Task<List<UserDto>> GetAllActiveUsersAsync(int page, int size)
        {
            List<AppUserEnt> users;
            if (page == 0 && size == 0)
            {
                users = await _userManager.Users.ToListAsync();
            }
            else
            {
                users = await _userManager.Users
                                .Skip(page * size)
                                .Take(size)
                                .ToListAsync();
            }

            return users
                .Where(user => user.DismissalDate == null || user.DismissalDate > DateTime.Now)
                .Select(user => new UserDto
                {
                    ID = user.Id,
                    RegistrationNo = user.RegistrationNo,
                    UserName = user.UserName,
                    Name = user.Name,
                    Surname = user.Surname,
                    NameSurname = user.NameSurname,
                    PhoneNumber = user.PhoneNumber,
                    CompanyPhoneNumber = user.CompanyPhoneNumber,
                    IntercomNumber = user.IntercomNumber,
                    SAPUserName = user.SAPUserName,
                    ComputerName = user.ComputerName,
                    EmployeeClassification = user.EmployeeClassification,
                    HireDate = user.HireDate,
                    DismissalDate = user.DismissalDate,
                    Gender = user.Gender,
                    MaritalStatus = user.MaritalStatus,
                    BirthDate = user.BirthDate,
                    CardNumber = user.CardNumber,
                    Department = user.Department,
                    DepartmentLogs = user.DepartmentLogs,
                    Position = user.Position,
                    PositionLogs = user.PositionLogs,
                    IsAdmin = user.IsAdmin,
                    ResponsibleDepartment = user.ResponsibleDepartment
                }).ToList();
        }

        public long GetCurrentUserID()
            => _contextService.GetCurrentUserId();
    }
}
