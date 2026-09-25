namespace Bidesk.Application.DTOs.User.Token
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
        public string refreshToken { get; set; }
    }
}
