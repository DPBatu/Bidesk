using Bidesk.Application.DTOs.Primeng;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Bidesk.Application.Features.Common.GridLazyLoad
{
    public class LoadEventMetaReq
    {
        public int? first { get; set; }
        public int? last { get; set; }
        public int? rows { get; set; }
        public string? sortField { get; set; }
        public int? sortOrder { get; set; }
        public SortMetaReq[]? multiSortMeta { get; set; }
        public JsonObject? filters { get; set; }

        public LoadEventDto ToDto()
        {
            LoadEventDto result = new()
            {
                First = first ?? 0,
                Last = last ?? 0,
                Rows = rows ?? 0
            };

            if (!string.IsNullOrWhiteSpace(sortField))
            {
                result.Sorts.Add(new SortMetaDto
                {
                    Field = sortField.Split('.').Select(s => s.Trim()).ToArray(),
                    Order = sortOrder ?? 0
                });
            }

            if (multiSortMeta != null)
            {
                foreach (SortMetaReq sort in multiSortMeta)
                {
                    // Boş alanlı (sıralanamayan kolon) sort meta'larını yok say.
                    if (string.IsNullOrWhiteSpace(sort.field)) continue;
                    result.Sorts.Add(sort.ToDto());
                }
            }

            if (filters != null)
            {
                foreach (KeyValuePair<string, JsonNode?> filter in filters)
                {
                    FilterMetaReq[]? fieldFilters = filter.Value.Deserialize<FilterMetaReq[]>();
                    if (fieldFilters == null) continue;

                    FilterDto field = new()
                    {
                        FieldPath = filter.Key,
                        PropertyName = filter.Key.Split('.').Select(c => c.Trim()).ToArray()
                    };

                    foreach (FilterMetaReq fieldFilter in fieldFilters)
                    {
                        if (fieldFilter.value == null) continue;
                        if (fieldFilter.value is JsonElement elem &&
                            (elem.ValueKind == JsonValueKind.Null ||
                             elem.ValueKind == JsonValueKind.Undefined ||
                             (elem.ValueKind == JsonValueKind.Array && elem.GetArrayLength() == 0))) continue;

                        field.Filters.Add(fieldFilter.ToDto());
                    }

                    if (field.Filters.Count > 0)
                        result.Filters.Add(field);
                }
            }

            return result;
        }
    }
}
