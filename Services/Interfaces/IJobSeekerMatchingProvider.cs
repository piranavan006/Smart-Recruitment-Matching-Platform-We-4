using SmartRecruitment.API.Services;

namespace SmartRecruitment.API.Services.Interfaces
{
    public interface IJobSeekerMatchingProvider
    {
        Task<List<JobSeekerMatchingProfile>>
            GetAllProfilesAsync();

        Task<JobSeekerMatchingProfile?>
            GetProfileAsync(int jobSeekerId);
    }
}