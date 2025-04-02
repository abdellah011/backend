namespace Admin.Models
{
    public class UploadedFile
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public byte[] FileData { get; set; } = null!;
        public string ContentType { get; set; } = string.Empty;
        public DateTime UploadedOn { get; set; } = DateTime.Now;
    }
}
