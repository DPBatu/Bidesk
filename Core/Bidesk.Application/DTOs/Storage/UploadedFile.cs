namespace Bidesk.Application.DTOs.Storage
{
    public class UploadedFile
    {
        public string FrontEndId { get; set; }
        public string Path { get; set; }
        public string OriginalFileName { get; set; }
        public string NewFileName { get; set; }
        public string Extension { get; set; }
        public string ContentType { get; set; }
        public string ContentDisposition { get; set; }
        public int Size { get; set; }
    }
}
