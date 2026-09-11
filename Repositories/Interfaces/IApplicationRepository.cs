using SmartRecruitment.API.Models;

namespace SmartRecruitment.API.Repositories.Interfaces
{
    public interface IApplicationRepository
    {
        Task<Application?> GetByIdAsync(int applicationId);

        Task<List<Application>> GetByJobSeekerIdAsync(
            int jobSeekerId);

        Task<List<Application>> GetByJobIdAsync(
            int jobId);

        Task<bool> ExistsAsync(
            int jobSeekerId,
            int jobId);

        Task<Application> AddAsync(
            Application application);

        Task UpdateAsync(
            Application application);
    }
}