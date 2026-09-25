using Bidesk.Application.Abstraction.Services.Log;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bidesk.Persistence.Services.Log
{
    public class LogServiceFactory : ILogServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public LogServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ILogService GetLogService(DbContext context)
        {
            return _serviceProvider.GetService<BideskLogService>();
        }
    }
}
