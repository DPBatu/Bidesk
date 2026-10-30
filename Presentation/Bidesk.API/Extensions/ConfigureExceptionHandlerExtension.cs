using Bidesk.Persistence.Services.Log;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace Bidesk.API.Extensions
{
    static public class ConfigureExceptionHandlerExtension
    {
        public static void ConfigureExceptionHandler<T>(this WebApplication application, ILogger<T> logger)
        {
            application.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.ContentType = MediaTypeNames.Application.Json;

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        int? code = contextFeature.Error.Data["statusCode"] as int?;
                        int statusCode = code ?? (int)HttpStatusCode.InternalServerError;
                        context.Response.StatusCode = statusCode;
                        logger.LogError(contextFeature.Error.Message);
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            statusCode,
                            message = contextFeature.Error.Message,
                            title = contextFeature.Error.Data["title"] ?? "Sunucu Hatası",
                        }
                        ));

                        ErrorLogService errorLogService = new ErrorLogService();
                        await errorLogService.ErrorLogger(
                                $"{Environment.NewLine}[{DateTime.Now}] " +
                                $"Exception: {contextFeature.Error.Message} | " +
                                $"Endpoint: {contextFeature.Endpoint?.DisplayName} | " +
                                $"Title: {contextFeature.Error.Data["title"] ?? "Sunucu Hatası"} | " +
                                $"StackTrace: {contextFeature.Error.StackTrace}"
                            );
                        //await errorLogService.ErrorLogger($"{Environment.NewLine}[{DateTime.Now}] Exception: {contextFeature.Error.Message}{"|"}{contextFeature.Endpoint?.DisplayName}{"|"}{contextFeature.Error.Data["title"] ?? "Sunucu Hatası"}");
                    }
                });
            });
        }
    }
}
