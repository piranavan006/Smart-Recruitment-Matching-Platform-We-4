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
        Task<EmployerProfile?> GetByIdAsync(int employerProfileId);

        Task<EmployerProfile?> GetByUserIdAsync(int userId);

        Task<List<EmployerProfile>> GetAllAsync();

        Task<EmployerProfile> AddAsync(EmployerProfile profile);

        Task UpdateAsync(EmployerProfile profile);

        Task DeleteAsync(int employerProfileId);

        Task<bool> ExistsByUserIdAsync(int userId);
    }
}