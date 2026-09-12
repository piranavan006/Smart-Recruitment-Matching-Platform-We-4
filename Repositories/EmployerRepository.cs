using Microsoft.EntityFrameworkCore;
using SmartRecruitment.API.Data;
using SmartRecruitment.API.Models;
using SmartRecruitment.API.Repositories.Interfaces;

namespace SmartRecruitment.API.Repositories
{
    public class EmployerRepository : IEmployerRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployerRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EmployerProfile?> GetByIdAsync(
            int id)
        {
            return await _context.EmployerProfiles
                .Include(e => e.Jobs)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<EmployerProfile?> GetByUserIdAsync(
            string userId)
        {
            return await _context.EmployerProfiles
                .Include(e => e.Jobs)
                .FirstOrDefaultAsync(
                    e => e.UserId == userId);
        }

        public async Task<bool> ExistsByUserIdAsync(
            string userId)
        {
            return await _context.EmployerProfiles
                .AnyAsync(e => e.UserId == userId);
        }

        public async Task<EmployerProfile> CreateAsync(
            EmployerProfile employer)
        {
            _context.EmployerProfiles.Add(employer);

            await _context.SaveChangesAsync();

            return employer;
        }

        public async Task<EmployerProfile> UpdateAsync(
            EmployerProfile employer)
        {
            _context.EmployerProfiles.Update(employer);

            await _context.SaveChangesAsync();

            return employer;
        }

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