namespace SmartRecruitment.API.Models
{
    public class Application
    {
        public int ApplicationId { get; set; }

        public int JobId { get; set; }

        public int JobSeekerProfileId { get; set; }

        public string Status { get; set; } = "Applied";

        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Job? Job { get; set; }

        public JobSeekerProfile? JobSeekerProfile { get; set; }
        public int JobSeekerId { get; set; }

        public string Status { get; set; } = "Pending";

        public decimal? MatchScore { get; set; }

        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Job? Job { get; set; }

        public JobSeekerProfile? JobSeeker { get; set; }
    }
}