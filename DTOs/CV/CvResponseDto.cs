namespace SmartRecruitment.API.DTOs.CV
{
    public class CvResponseDto
    {
        public int JobSeekerProfileId { get; set; }

        public string CVFileName { get; set; } = string.Empty;

        public string CVFilePath { get; set; } = string.Empty;

        public DateTime? CVUploadedAt { get; set; }
    }
}