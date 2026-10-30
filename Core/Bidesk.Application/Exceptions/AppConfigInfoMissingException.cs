namespace Bidesk.Application.Exceptions
{
    public class AppConfigInfoMissingException: BaseException
    {
        public AppConfigInfoMissingException(string message) : base(message, "AppSetting Eksik Parametre!")
        {
        }
    }
}
