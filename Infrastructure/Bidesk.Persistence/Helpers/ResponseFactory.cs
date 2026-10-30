using Bidesk.Application.Features;

namespace Bidesk.Persistence.Helpers
{
    /// <summary>Servislerin BaseRes turevi yanitlarini tek bicimde uretir.</summary>
    public static class ResponseFactory
    {
        public static T Success<T>(string message = "", Action<T>? configure = null) where T : BaseRes, new()
        {
            var response = new T { IsSuccess = true, Message = message };
            configure?.Invoke(response);
            return response;
        }

        /// <summary>Kullaniciya gosterilecek is kurali hatasi.</summary>
        public static T Fail<T>(string message) where T : BaseRes, new()
            => new() { IsSuccess = false, Message = message, ErrorMessage = message };

        public static T Error<T>(Exception ex) where T : BaseRes, new()
            => new()
            {
                IsSuccess = false,
                Message = "İşlem Başarısız",
                ErrorMessage = $"{ex.Message} {ex.InnerException?.Message ?? string.Empty}".Trim()
            };
    }
}
