using Bidesk.API.Extensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bidesk.Application.Behaviors
{
    public class ExceptionLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ILogger<ExceptionLoggingBehavior<TRequest, TResponse>> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ExceptionLoggingBehavior(
            ILogger<ExceptionLoggingBehavior<TRequest, TResponse>> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception exception)
            {
                var requestName = typeof(TRequest).Name;
                
                _logger.LogError(exception, "MediatR Request Exception: {RequestName} {@Request}", requestName, request);
                
                // GlobalExceptionHandler ile ayný logging mekanizmasýný kullan
                try
                {
                    var exceptionLogger = _serviceProvider.GetService<IUnhandledExceptionLogger>();
                    if (exceptionLogger != null)
                    {
                        await exceptionLogger.LogUnhandledExceptionAsync(exception, $"MediatR Handler: {requestName}");
                    }
                }
                catch (Exception logException)
                {
                    _logger.LogError(logException, "Error occurred while logging MediatR exception");
                    Console.WriteLine($"MediatR exception logging failed: {logException.Message}");
                }
                
                // Exception'ý tekrar fýrlat ki normal flow devam etsin
                throw;
            }
        }
    }
}