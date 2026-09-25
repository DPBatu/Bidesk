namespace Bidesk.Application.DTOs.Primeng
{
    public class FilterDto
    {
        public string FieldPath { get; set; } = string.Empty;
        public string[] PropertyName { get; set; } = Array.Empty<string>();
        public List<FilterValueDto> Filters { get; set; } = new();
    }
}
