using Bidesk.Domain.Entities;

namespace Bidesk.Domain.AuditLog
{
    public class BaseAuditLog : BaseEntity
    {
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public long TableID { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }

    }
}
