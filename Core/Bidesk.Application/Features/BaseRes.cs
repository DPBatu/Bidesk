namespace Bidesk.Application.Features
{
    public class BaseRes
    {
        public long Id { get; set; }
        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
