namespace Bidesk.Application.Abstraction.Services
{
    public interface IWhatsAppService
    {
        public Task SendMessageAsync(string phoneNumber, string message);
    }
}
