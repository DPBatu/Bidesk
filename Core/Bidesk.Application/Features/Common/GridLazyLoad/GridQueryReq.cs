using Bidesk.Application.DTOs.Primeng;
using System.Text.Json.Serialization;

namespace Bidesk.Application.Features.Common.GridLazyLoad
{
    public class GridQueryReq
    {
        [JsonPropertyName("skipPagination")]
        public bool? SkipPagination { get; set; }
        [JsonPropertyName("search")]
        public string? Search { get; set; }
        [JsonPropertyName("loadEvent")]
        public LoadEventMetaReq? LoadEvent { get; set; }

        public GridQueryDto ToDto() => new()
        {
            SkipPagination = SkipPagination ?? false,
            Search = string.IsNullOrWhiteSpace(Search) ? null : Search.Trim(),
            LoadEvent = LoadEvent?.ToDto()
        };
    }
}
