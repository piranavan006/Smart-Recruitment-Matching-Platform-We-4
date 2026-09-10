using SmartRecruitment.API.Models;

namespace SmartRecruitment.API.Repositories.Interfaces
{
    public interface IJobRepository
    {
        Task<Job?> GetByIdAsync(int jobId);

        Task<List<Job>> GetAllAsync();

        Task<List<Job>> GetByEmployerIdAsync(int employerProfileId);

        Task<List<Job>> GetActiveJobsAsync();

        Task<Job> AddAsync(Job job);

        Task UpdateAsync(Job job);

        Task DeleteAsync(int jobId);

        Task<bool> ExistsByIdAsync(int jobId);
    }
}