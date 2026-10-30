using Bidesk.Application.DTOs.User.Token;

namespace Bidesk.Application.ViewModels.User
{
    public class LoginInfoVm
    {
        public TokenDto Token { get; set; }
        public UserInfoVm UserInfo { get; set; }
    }
}
