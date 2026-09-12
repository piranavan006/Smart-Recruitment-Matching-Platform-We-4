using SmartRecruitment.API.Models;

namespace SmartRecruitment.API.Repositories.Interfaces
{
    public interface IEmployerRepository
    {
        Task<EmployerProfile?> GetByIdAsync(int id);

        Task<EmployerProfile?> GetByUserIdAsync(string userId);

        Task<bool> ExistsByUserIdAsync(string userId);

        Task<EmployerProfile> CreateAsync(
            EmployerProfile employer);

        Task<EmployerProfile> UpdateAsync(
            EmployerProfile employer);

        Task<bool> HasJobsAsync(int employerProfileId);
    }
}