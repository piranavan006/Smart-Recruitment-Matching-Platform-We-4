using SmartRecruitment.API.Models;

namespace SmartRecruitment.API.Repositories.Interfaces
{
    public interface ICVRepository
    {
        Task<CV?> GetByUserIdAsync(int userId);

        Task<CV?> GetByIdAsync(int cvId);

        Task<CV> AddAsync(CV cv);

        Task UpdateAsync(CV cv);

        Task DeleteAsync(int cvId);
    }
}
