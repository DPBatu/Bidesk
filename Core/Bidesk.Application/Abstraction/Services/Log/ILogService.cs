using Bidesk.Domain.AuditLog;
using Microsoft.EntityFrameworkCore;

namespace Bidesk.Application.Abstraction.Services.Log
{
    public interface ILogService
    {
        Task LogChangesAsync(DbContext context, IEnumerable<BaseAuditLog> logEntries);
    }
}
