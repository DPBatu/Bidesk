using Bidesk.Application.Features;

namespace Bidesk.Application.Abstraction.Services
{
    public interface ISmsService
    {
        public Task<BaseRes> SendSmsAsync(string phoneNumber, string message);
    }
}
