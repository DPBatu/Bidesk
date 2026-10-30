using Bidesk.Application.DTOs.Primeng;
using System.Text.Json;

namespace Bidesk.Application.Features.Common.GridLazyLoad
{
    public class FilterMetaReq
    {
        public object? value { get; set; }
        public string matchMode { get; set; } = string.Empty;
        public string @operator { get; set; } = string.Empty;

        public FilterValueDto ToDto()
        {
            FilterValueDto result = new()
            {
                MatchMode = matchMode,
                Operator = @operator
            };

            if (value is JsonElement element && element.ValueKind == JsonValueKind.Array)
            {
                result.Values = element.EnumerateArray()
                    .Select(e => e.ValueKind == JsonValueKind.String ? e.GetString() ?? string.Empty : e.ToString())
                    .ToArray();
                result.Value = result.Values.FirstOrDefault() ?? string.Empty;
            }
            else
            {
                result.Value = value?.ToString() ?? string.Empty;
            }

            return result;
        }
    }
}
