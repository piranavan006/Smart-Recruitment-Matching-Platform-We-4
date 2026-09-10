namespace SmartRecruitment.API.DTOs.Applications
{
    public class ApplicationResponseDto
    {
        public int ApplicationId { get; set; }

        public int JobId { get; set; }

        public int JobSeekerId { get; set; }

        public string Status { get; set; } = string.Empty;

        public decimal? MatchScore { get; set; }

        public DateTime AppliedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}