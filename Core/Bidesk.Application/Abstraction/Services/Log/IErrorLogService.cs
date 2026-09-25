namespace Bidesk.Application.Abstraction.Services.Log
{
    public interface IErrorLogService
    {
        public Task ErrorLogger(string _errorMessage);
        public Task HandleLogError(Exception ex);
    }
}
