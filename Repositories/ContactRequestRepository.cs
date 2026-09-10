using Microsoft.EntityFrameworkCore;
using SmartRecruitment.API.Data;
using SmartRecruitment.API.Models;
using SmartRecruitment.API.Repositories.Interfaces;

namespace SmartRecruitment.API.Repositories
{
    public class ContactRequestRepository
        : IContactRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactRequestRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ContactRequest?>
            GetByIdAsync(int contactRequestId)
        {
            return await _context.ContactRequests
                .FirstOrDefaultAsync(
                    x => x.ContactRequestId ==
                         contactRequestId);
        }

        public async Task<List<ContactRequest>>
            GetAllAsync()
        {
            return await _context.ContactRequests
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<ContactRequest>>
            GetBySenderIdAsync(int senderId)
        {
            return await _context.ContactRequests
                .Where(x => x.SenderId == senderId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<ContactRequest>>
            GetByReceiverIdAsync(int receiverId)
        {
            return await _context.ContactRequests
                .Where(x => x.ReceiverId == receiverId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> ExistsPendingAsync(
            int senderId,
            int receiverId)
        {
            return await _context.ContactRequests
                .AnyAsync(x =>
                    x.SenderId == senderId &&
                    x.ReceiverId == receiverId &&
                    x.Status == "Pending");
        }

        public async Task<ContactRequest>
            AddAsync(ContactRequest contactRequest)
        {
            await _context.ContactRequests
                .AddAsync(contactRequest);

            await _context.SaveChangesAsync();

            return contactRequest;
        }

        public async Task UpdateAsync(
            ContactRequest contactRequest)
        {
            _context.ContactRequests
                .Update(contactRequest);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(
            int contactRequestId)
        {
            var request =
                await GetByIdAsync(contactRequestId);

            if (request == null)
                return;

            _context.ContactRequests
                .Remove(request);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(
            int contactRequestId)
        {
            return await _context.ContactRequests
                .AnyAsync(x =>
                    x.ContactRequestId ==
                    contactRequestId);
        }
    }
}