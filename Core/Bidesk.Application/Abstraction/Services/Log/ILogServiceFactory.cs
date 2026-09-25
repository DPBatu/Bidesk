using Microsoft.EntityFrameworkCore;

namespace Bidesk.Application.Abstraction.Services.Log
{
    public interface ILogServiceFactory
    {
        ILogService GetLogService(DbContext context);
    }
}
