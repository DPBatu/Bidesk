using Bidesk.Application.DTOs.User.Token;
using Bidesk.Application.ViewModels.Company;
using Bidesk.Domain.Entities.Public;

namespace Bidesk.Application.Abstraction.Services.User
{
    public interface ITokenHandler
    {
        TokenDto CreateAccessToken(int second, AppUserEnt appUser, CompanyVm company);
        string CreateRefreshToken();
    }
}
