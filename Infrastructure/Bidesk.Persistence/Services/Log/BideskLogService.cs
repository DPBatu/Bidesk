using Bidesk.Application.Abstraction.Services.Log;
using Bidesk.Application.Abstraction.Services.User;
using Bidesk.Domain.AuditLog;
using Bidesk.Domain.Entities;
using Bidesk.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Bidesk.Persistence.Services.Log
{
    public class BideskLogService : ILogService
    {
        private readonly BaseContext _context;
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;

        public BideskLogService(BaseContext context, IMailService mailService, IConfiguration configuration)
        {
            _context = context;
            _mailService = mailService;
            _configuration = configuration;
        }

        public async Task LogChangesAsync(DbContext context, IEnumerable<BaseAuditLog> logEntries)
        {
            try
            {
                List<AuditLogEnt> auditLogs = logEntries.Select(logEntry => new AuditLogEnt
                {
                    TableName = logEntry.TableName,
                    FieldName = logEntry.FieldName,
                    TableID = logEntry.TableID,
                    OldValue = logEntry.OldValue,
                    NewValue = logEntry.NewValue,
                    CreatedDate = logEntry.CreatedDate,
                    CreatedUserId = logEntry.CreatedUserId
                }).ToList();

                _context.AuditLogs.AddRange(auditLogs);
                _context.SaveChanges();
            }
            catch (Exception Ex)
            {
                _mailService.SendMailAsync(_configuration["BideskErrMail:MailAddr"].ToString(), "Bidesk hata - Bidesk Log", $"{Ex.Message} {(Ex.InnerException != null ? Ex.InnerException.Message : string.Empty)}");
                throw Ex;
            }
        }
    }
}
