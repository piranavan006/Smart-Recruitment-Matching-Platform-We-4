namespace SmartRecruitment.API.Services
{
    public class JobSeekerMatchingProfile
    {
        public int JobSeekerId { get; set; }

        public string? FullName { get; set; }

        public List<string> Skills { get; set; }
            = new List<string>();

        public int ExperienceYears { get; set; }

        public string? Education { get; set; }

        public string? Location { get; set; }
    }
}