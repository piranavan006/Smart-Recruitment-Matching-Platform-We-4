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
        public EmployerRepository(ApplicationDbContext context)
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
        public async Task<EmployerProfile?> GetByIdAsync(int employerProfileId)
        {
            return await _context.EmployerProfiles
                .FirstOrDefaultAsync(e =>
                    e.EmployerProfileId == employerProfileId);
        }

        public async Task<EmployerProfile?> GetByUserIdAsync(int userId)
        {
            return await _context.EmployerProfiles
                .FirstOrDefaultAsync(e =>
                    e.UserId == userId);
        }

        public async Task<List<EmployerProfile>> GetAllAsync()
        {
            return await _context.EmployerProfiles
                .ToListAsync();
        }

        public async Task<EmployerProfile> AddAsync(
            EmployerProfile profile)
        {
            await _context.EmployerProfiles.AddAsync(profile);
            await _context.SaveChangesAsync();

            return profile;
        }

        public async Task UpdateAsync(EmployerProfile profile)
        {
            _context.EmployerProfiles.Update(profile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int employerProfileId)
        {
            var profile = await _context.EmployerProfiles
                .FirstOrDefaultAsync(e =>
                    e.EmployerProfileId == employerProfileId);

            if (profile == null)
                return;

            _context.EmployerProfiles.Remove(profile);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByUserIdAsync(int userId)
        {
            return await _context.EmployerProfiles
                .AnyAsync(e => e.UserId == userId);
        }
    }
}