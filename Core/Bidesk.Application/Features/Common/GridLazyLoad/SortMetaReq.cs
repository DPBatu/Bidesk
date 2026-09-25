using Bidesk.Application.DTOs.Primeng;

namespace Bidesk.Application.Features.Common.GridLazyLoad
{
    public class SortMetaReq
    {
        public string? field { get; set; } = string.Empty;
        public int order { get; set; }

        public SortMetaDto ToDto() => new()
        {
            // Sıralanamayan kolonlara (ör. seçim/aksiyon) tıklanınca PrimeNG field=null
            // gönderebilir; boş Field dizisi handler tarafından güvenle yok sayılır.
            Field = string.IsNullOrWhiteSpace(field)
                ? Array.Empty<string>()
                : field.Split('.').Select(s => s.Trim()).ToArray(),
            Order = order
        };
    }
}
