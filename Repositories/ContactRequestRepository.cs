using Microsoft.EntityFrameworkCore;
using SmartRecruitment.API.Data;
using SmartRecruitment.API.Models;
using SmartRecruitment.API.Repositories.Interfaces;

namespace SmartRecruitment.API.Repositories
{
    public class ContactRequestRepository : IContactRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ContactRequest?> GetByIdAsync(int contactRequestId)
        {
            return await _context.ContactRequests
                .FirstOrDefaultAsync(c => c.ContactRequestId == contactRequestId);
        }

        public async Task<List<ContactRequest>> GetAllAsync()
        {
            return await _context.ContactRequests
                .ToListAsync();
        }

        public async Task<List<ContactRequest>> GetBySenderIdAsync(int senderId)
        {
            return await _context.ContactRequests
                .Where(c => c.SenderId == senderId)
                .ToListAsync();
        }

        public async Task<List<ContactRequest>> GetByReceiverIdAsync(int receiverId)
        {
            return await _context.ContactRequests
                .Where(c => c.ReceiverId == receiverId)
                .ToListAsync();
        }

        public async Task<ContactRequest> AddAsync(ContactRequest contactRequest)
        {
            await _context.ContactRequests.AddAsync(contactRequest);
            await _context.SaveChangesAsync();

            return contactRequest;
        }

        public async Task UpdateAsync(ContactRequest contactRequest)
        {
            _context.ContactRequests.Update(contactRequest);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int contactRequestId)
        {
            var contactRequest = await _context.ContactRequests
                .FirstOrDefaultAsync(c =>
                    c.ContactRequestId == contactRequestId);

            if (contactRequest == null)
                return;

            _context.ContactRequests.Remove(contactRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(int contactRequestId)
        {
            return await _context.ContactRequests
                .AnyAsync(c => c.ContactRequestId == contactRequestId);
        }
    }
}