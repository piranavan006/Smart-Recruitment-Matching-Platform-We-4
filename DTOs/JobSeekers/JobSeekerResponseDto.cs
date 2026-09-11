namespace SmartRecruitment.API.DTOs.JobSeekers
{
    public class JobSeekerResponseDto
    {
        public int JobSeekerProfileId { get; set; }

        public int UserId { get; set; }

        public string Summary { get; set; } = string.Empty;

        public string Skills { get; set; } = string.Empty;

        public string Experience { get; set; } = string.Empty;

        public string Education { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string? CVFileName { get; set; }

        public string? CVFilePath { get; set; }

        public DateTime? CVUploadedAt { get; set; }
    }
}