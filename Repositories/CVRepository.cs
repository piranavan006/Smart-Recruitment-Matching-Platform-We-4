using Microsoft.EntityFrameworkCore;
using SmartRecruitment.API.Data;
using SmartRecruitment.API.Models;
using SmartRecruitment.API.Repositories.Interfaces;

namespace SmartRecruitment.API.Repositories
{
    public class CVRepository : ICVRepository
    {
        private readonly ApplicationDbContext _context;

        public CVRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CV?> GetByUserIdAsync(int userId)
        {
            return await _context.CVs
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<CV?> GetByIdAsync(int cvId)
        {
            return await _context.CVs
                .FirstOrDefaultAsync(x => x.CVId == cvId);
        }

        public async Task<CV> AddAsync(CV cv)
        {
            await _context.CVs.AddAsync(cv);
            await _context.SaveChangesAsync();

            return cv;
        }

        public async Task UpdateAsync(CV cv)
        {
            _context.CVs.Update(cv);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int cvId)
        {
            var cv = await _context.CVs
                .FirstOrDefaultAsync(x => x.CVId == cvId);

            if (cv != null)
            {
                _context.CVs.Remove(cv);
                await _context.SaveChangesAsync();
            }
        }
    }
}
