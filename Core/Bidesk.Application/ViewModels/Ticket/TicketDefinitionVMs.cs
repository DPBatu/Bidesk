namespace Bidesk.Application.ViewModels.Ticket
{
    public class TicketStatusVM
    {
        public long ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Severity { get; set; } = "info";
        public int SortOrder { get; set; }
        public bool IsDefault { get; set; }
        public bool IsResolved { get; set; }
        public bool IsClosed { get; set; }
        public bool PausesSla { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class TicketPriorityVM
    {
        public long ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Severity { get; set; } = "secondary";
        public int SortOrder { get; set; }
        public bool IsDefault { get; set; }
        public int? ResolveHours { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class TechnicianVM
    {
        public long UserID { get; set; }
        public string? NameSurname { get; set; }
    }

    public class TicketGroupVM
    {
        public long ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public List<TechnicianVM> Technicians { get; set; } = new();
    }

    public class TicketSubCategoryVM
    {
        public long ID { get; set; }
        public long CategoryID { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    public class TicketCategoryVM
    {
        public long ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public List<TicketSubCategoryVM> SubCategories { get; set; } = new();
    }

    /// <summary>Talep formu ve listeler icin tek seferde gelen aktif tanimlar.</summary>
    public class TicketLookupsVM
    {
        public List<TicketStatusVM> Statuses { get; set; } = new();
        public List<TicketPriorityVM> Priorities { get; set; } = new();
        public List<TicketGroupVM> Groups { get; set; } = new();
        public List<TicketCategoryVM> Categories { get; set; } = new();

        /// <summary>Oturumdaki kullanici en az bir grupta teknisyen mi (ya da admin mi).</summary>
        public bool IsTechnician { get; set; }
    }

    public class BideskParameterVM
    {
        public string ParamName { get; set; } = string.Empty;
        public string? ParamValue { get; set; }
    }
}
