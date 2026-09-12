using SmartRecruitment.API.Models;

namespace SmartRecruitment.API.Repositories.Interfaces
{
    public interface IJobRepository
    {
        Task<Job?> GetByIdAsync(int jobId);

        Task<List<Job>> GetAllAsync();

        Task<List<Job>> GetByEmployerIdAsync(
            int employerProfileId);

        Task<List<Job>> GetActiveJobsAsync();

        Task<List<Job>> SearchAsync(
            string? keyword,
            string? location,
            string? education,
            int? minExperienceYears,
            int? maxExperienceYears,
            decimal? salaryMin,
            decimal? salaryMax);

        Task<Job> AddAsync(Job job);

        Task<Job> UpdateAsync(Job job);

        Task DeleteAsync(int jobId);

        Task<bool> ExistsByIdAsync(int jobId);

        Task<bool> CloseAsync(int jobId);
    }
}