namespace Bidesk.Application.DTOs.Primeng
{
    public class LoadEventDto
    {
        public int First { get; set; }
        public int Last { get; set; }
        public int Rows { get; set; }
        public List<FilterDto> Filters { get; set; } = new();
        public List<SortMetaDto> Sorts { get; set; } = new();
    }
}
