namespace Bidesk.Application.DTOs.Primeng
{
    public class GridQueryDto
    {
        public bool SkipPagination { get; set; } = false;
        public string? Search { get; set; }
        public LoadEventDto? LoadEvent { get; set; }
    }
}
