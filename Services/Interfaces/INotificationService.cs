using SmartRecruitment.API.Models;

namespace SmartRecruitment.API.Services.Interfaces
{
    public interface INotificationService
    {
        Task<Notification?> GetByIdAsync(int notificationId);

        Task<List<Notification>> GetAllAsync();

        Task<List<Notification>> GetByUserIdAsync(int userId);

        Task<Notification> AddAsync(Notification notification);

        Task UpdateAsync(Notification notification);

        Task DeleteAsync(int notificationId);

        Task<bool> ExistsByIdAsync(int notificationId);
    }
}