using SmartRecruitment.API.Models;

namespace SmartRecruitment.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int userId);

        Task<User?> GetByEmailAsync(string email);

        Task<List<User>> GetAllAsync();

        Task<User> AddAsync(User user);

        Task UpdateAsync(User user);

        Task DeleteAsync(int userId);

        Task<bool> ExistsByEmailAsync(string email);
    }
}