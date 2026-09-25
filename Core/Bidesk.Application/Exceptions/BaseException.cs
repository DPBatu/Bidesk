namespace Bidesk.Application.Exceptions
{
    public class BaseException : Exception
    {
        public BaseException(int? statusCode = 599) : base("Sunucu beklenmedik bir hata ile karşılaştı!") { Data.Add("statusCode", 599); }
        public BaseException(string? message, int statusCode = 599) : base(message) { Data.Add("statusCode", 599); }
        public BaseException(string? message, string? title, int statusCode = 599) : base(message) { Data.Add("title", title); Data.Add("statusCode", statusCode); }
        public BaseException(Exception exception) : base()
        {
            string? message = exception?.Message;
            string? innerMessage = exception?.InnerException?.Message;
            string? fullMessage = string.IsNullOrEmpty(innerMessage) ? message : message + "-" + innerMessage;

            BaseException baseException = new BaseException(fullMessage);
            baseException.Data.Add("title", exception?.Data["title"]?.ToString() ?? "Sunucu Hatası!");

            throw baseException;
        }
    }
}
