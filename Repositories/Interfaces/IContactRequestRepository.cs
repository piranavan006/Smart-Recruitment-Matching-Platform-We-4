using SmartRecruitment.API.Models;

namespace SmartRecruitment.API.Repositories.Interfaces
{
    public interface IContactRequestRepository
    {
        Task<ContactRequest?> GetByIdAsync(
            int contactRequestId);

        Task<List<ContactRequest>> GetAllAsync();

        Task<List<ContactRequest>> GetBySenderIdAsync(
            int senderId);

        Task<List<ContactRequest>> GetByReceiverIdAsync(
            int receiverId);

        Task<bool> ExistsPendingAsync(
            int senderId,
            int receiverId);

        Task<ContactRequest> AddAsync(
            ContactRequest contactRequest);

        Task UpdateAsync(
            ContactRequest contactRequest);

        Task DeleteAsync(
            int contactRequestId);

        Task<bool> ExistsByIdAsync(
            int contactRequestId);
    }
}