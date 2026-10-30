using Bidesk.Application.DTOs.Primeng;
using Bidesk.Application.Features.Common.GridLazyLoad;
using System.Text.Json;

namespace Bidesk.Application.Extensions
{
    public static class GridQueryExtensions
    {
        public static GridQueryDto ToGridQueryDto(this string str)
        {
            if (string.IsNullOrEmpty(str)) return new();
            GridQueryReq? req = JsonSerializer.Deserialize<GridQueryReq>(str);
            return req != null ? req.ToDto() : new();
        }
    }
}
