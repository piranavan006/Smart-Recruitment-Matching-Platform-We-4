namespace SmartRecruitment.API.Models
{
    public class CV
    {
        public int CVId { get; set; }

        public int UserId { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

        public DateTime UploadedAt { get; set; }

        // Navigation property
        public User? User { get; set; }
    }
}
