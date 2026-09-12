namespace SmartRecruitment.API.Models
{
    public class Application
    {
        public int ApplicationId { get; set; }

        public int JobId { get; set; }

        public int JobSeekerId { get; set; }

        

        public decimal? MatchScore { get; set; }

        

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        
        // Navigation properties
        public Job? Job { get; set; }

        public JobSeekerProfile? JobSeeker { get; set; }
    }
}