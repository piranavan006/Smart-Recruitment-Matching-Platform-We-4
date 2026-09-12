using SmartRecruitment.API.Models;

namespace SmartRecruitment.API.Repositories.Interfaces
{
    public interface IJobRepository
    {
        Task<Job?> GetByIdAsync(int id);

        Task<List<Job>> GetByEmployerIdAsync(
            int employerProfileId);

        Task<Job> CreateAsync(Job job);

        Task<Job> UpdateAsync(Job job);

        Task<bool> CloseAsync(int id);

        Task<List<Job>> SearchAsync(
            string? keyword,
            string? location,
            string? education,
            int? minExperienceYears,
            int? maxExperienceYears,
            decimal? salaryMin,
            decimal? salaryMax);
    }
}