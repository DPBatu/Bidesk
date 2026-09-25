namespace Bidesk.Application.Exceptions
{
    public class MissingInfoException: BaseException
    {
        public MissingInfoException(string message): base(message, "Eksik Bilgi!")
        {
        }
    }
}
