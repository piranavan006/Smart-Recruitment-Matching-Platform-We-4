using SmartRecruitment.API.Models;

namespace SmartRecruitment.API.Repositories.Interfaces
{
    public interface IEmployerRepository
    {
        // Get employer by user ID
        Task<EmployerProfile?> GetByUserIdAsync(string userId);

        // Get employer by profile ID
        Task<EmployerProfile?> GetByIdAsync(
            int employerProfileId);

        // Get all employer profiles
        Task<List<EmployerProfile>> GetAllAsync();

        // Create employer profile
        Task<EmployerProfile> CreateAsync(
            EmployerProfile employer);

        // Update employer profile
        Task<EmployerProfile> UpdateAsync(
            EmployerProfile employer);

        // Delete employer profile
        Task DeleteAsync(
            int employerProfileId);

        // Check whether employer exists
        Task<bool> ExistsByUserIdAsync(
            string userId);
    }
}