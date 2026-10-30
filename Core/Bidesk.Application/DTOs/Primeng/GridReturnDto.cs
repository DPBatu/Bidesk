namespace Bidesk.Application.DTOs.Primeng
{
    public class GridReturnDto<T> where T : class
    {
        public List<T> Data { get; set; } = new();
        public long TotalRecords { get; set; }
    }
}
