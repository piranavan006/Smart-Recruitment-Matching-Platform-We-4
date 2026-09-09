using SmartRecruitment.API.Models;
using SmartRecruitment.API.Repositories.Interfaces;
using SmartRecruitment.API.Services.Interfaces;

namespace SmartRecruitment.API.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<Notification?> GetByIdAsync(int notificationId)
        {
            return await _notificationRepository.GetByIdAsync(notificationId);
        }

        public async Task<List<Notification>> GetAllAsync()
        {
            return await _notificationRepository.GetAllAsync();
        }

        public async Task<List<Notification>> GetByUserIdAsync(int userId)
        {
            return await _notificationRepository.GetByUserIdAsync(userId);
        }

        public async Task<Notification> AddAsync(Notification notification)
        {
            return await _notificationRepository.AddAsync(notification);
        }

        public async Task UpdateAsync(Notification notification)
        {
            await _notificationRepository.UpdateAsync(notification);
        }

        public async Task DeleteAsync(int notificationId)
        {
            await _notificationRepository.DeleteAsync(notificationId);
        }

        public async Task<bool> ExistsByIdAsync(int notificationId)
        {
            return await _notificationRepository.ExistsByIdAsync(notificationId);
        }
    }
}