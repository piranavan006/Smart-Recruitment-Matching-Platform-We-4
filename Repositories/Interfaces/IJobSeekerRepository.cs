using SmartRecruitment.API.Models;

namespace SmartRecruitment.API.Repositories.Interfaces
{
    public interface IJobSeekerRepository
    {
        Task<JobSeekerProfile?> GetByIdAsync(int jobSeekerProfileId);

        Task<JobSeekerProfile?> GetByUserIdAsync(int userId);

        Task<List<JobSeekerProfile>> GetAllAsync();

        Task<JobSeekerProfile> AddAsync(JobSeekerProfile profile);

        Task UpdateAsync(JobSeekerProfile profile);

        Task DeleteAsync(int jobSeekerProfileId);

        Task<bool> ExistsByUserIdAsync(int userId);
    }
}