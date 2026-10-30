namespace Bidesk.Application.DTOs.Primeng
{
    public class FilterValueDto
    {
        public string MatchMode { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string[]? Values { get; set; }
        public string Operator { get; set; } = string.Empty;
    }
}
