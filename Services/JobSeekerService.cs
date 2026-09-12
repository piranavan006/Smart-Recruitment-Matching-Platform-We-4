namespace SmartRecruitment.API.Services.Interfaces
{
    public interface IJobSeekerMatchingProvider
    {
        Task<List<JobSeekerMatchingProfile>>
            GetAllProfilesAsync();

        Task<JobSeekerMatchingProfile?>
            GetProfileAsync(int jobSeekerId);
    }

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