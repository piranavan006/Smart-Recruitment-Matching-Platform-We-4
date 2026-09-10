using SmartRecruitment.API.Models;

namespace SmartRecruitment.API.Repositories.Interfaces
{
    public interface IEmployerRepository
    {
        Task<EmployerProfile?> GetByIdAsync(int employerProfileId);

        Task<EmployerProfile?> GetByUserIdAsync(int userId);

        Task<List<EmployerProfile>> GetAllAsync();

        Task<EmployerProfile> AddAsync(EmployerProfile profile);

        Task UpdateAsync(EmployerProfile profile);

        Task DeleteAsync(int employerProfileId);

        Task<bool> ExistsByUserIdAsync(int userId);
    }
}