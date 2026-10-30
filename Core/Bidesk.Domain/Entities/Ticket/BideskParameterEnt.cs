using System.ComponentModel.DataAnnotations;

namespace Bidesk.Domain.Entities.Ticket
{
    /// <summary>Anahtar/deger parametreleri (orn. AutoCloseDays).</summary>
    public class BideskParameterEnt : BaseEntity
    {
        [MaxLength(50)]
        public string ParamName { get; set; } = null!;

        [MaxLength(250)]
        public string? ParamValue { get; set; }
    }
}
