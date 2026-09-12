using Microsoft.EntityFrameworkCore;
using SmartRecruitment.API.Data;
using SmartRecruitment.API.Models;
using SmartRecruitment.API.Repositories.Interfaces;

namespace SmartRecruitment.API.Repositories
{
    public class EmployerRepository : IEmployerRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // GET BY ID
        // =====================================================
        public async Task<EmployerProfile?> GetByIdAsync(int id)
        {
            return await _context.EmployerProfiles
                .Include(e => e.Jobs)
                .FirstOrDefaultAsync(
                    e => e.EmployerProfileId == id);
        }

        // =====================================================
        // GET BY USER ID
        // =====================================================
        public async Task<EmployerProfile?> GetByUserIdAsync(
            string userId)
        {
            return await _context.EmployerProfiles
                .Include(e => e.Jobs)
                .FirstOrDefaultAsync(
                    e => e.UserId == userId);
        }

        // =====================================================
        // GET ALL EMPLOYERS
        // =====================================================
        public async Task<List<EmployerProfile>> GetAllAsync()
        {
            return await _context.EmployerProfiles
                .Include(e => e.Jobs)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        // =====================================================
        // EXISTS BY USER ID
        // =====================================================
        public async Task<bool> ExistsByUserIdAsync(
            string userId)
        {
            return await _context.EmployerProfiles
                .AnyAsync(e => e.UserId == userId);
        }

        // =====================================================
        // CREATE
        // =====================================================
        public async Task<EmployerProfile> CreateAsync(
            EmployerProfile employer)
        {
            _context.EmployerProfiles.Add(employer);

            await _context.SaveChangesAsync();

            return employer;
        }

        // =====================================================
        // UPDATE
        // =====================================================
        public async Task<EmployerProfile> UpdateAsync(
            EmployerProfile employer)
        {
            _context.EmployerProfiles.Update(employer);

            await _context.SaveChangesAsync();

            return employer;
        }

        // =====================================================
        // DELETE
        // =====================================================
        public async Task DeleteAsync(int employerProfileId)
        {
            var employer =
                await _context.EmployerProfiles
                    .FirstOrDefaultAsync(
                        e => e.EmployerProfileId ==
                             employerProfileId);

            if (employer == null)
            {
                return;
            }

            _context.EmployerProfiles.Remove(employer);

            await _context.SaveChangesAsync();
        }

        // =====================================================
        // HAS JOBS
        // =====================================================
        public async Task<bool> HasJobsAsync(
            int employerProfileId)
        {
            return await _context.Jobs
                .AnyAsync(j =>
                    j.EmployerProfileId ==
                    employerProfileId);
        }
    }
}