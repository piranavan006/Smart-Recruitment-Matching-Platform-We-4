using Microsoft.EntityFrameworkCore;
using SmartRecruitment.API.Data;
using SmartRecruitment.API.Models;
using SmartRecruitment.API.Repositories.Interfaces;

namespace SmartRecruitment.API.Repositories
{
    public class JobSeekerRepository : IJobSeekerRepository
    {
        private readonly ApplicationDbContext _context;

        public JobSeekerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<JobSeekerProfile?> GetByIdAsync(int jobSeekerProfileId)
        {
            return await _context.JobSeekerProfiles
                .FirstOrDefaultAsync(p =>
                    p.JobSeekerProfileId == jobSeekerProfileId);
        }

        public async Task<JobSeekerProfile?> GetByUserIdAsync(int userId)
        {
            return await _context.JobSeekerProfiles
                .FirstOrDefaultAsync(p =>
                    p.UserId == userId);
        }

        public async Task<List<JobSeekerProfile>> GetAllAsync()
        {
            return await _context.JobSeekerProfiles
                .ToListAsync();
        }

        public async Task<JobSeekerProfile> AddAsync(
            JobSeekerProfile profile)
        {
            await _context.JobSeekerProfiles.AddAsync(profile);
            await _context.SaveChangesAsync();

            return profile;
        }

        public async Task UpdateAsync(JobSeekerProfile profile)
        {
            _context.JobSeekerProfiles.Update(profile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int jobSeekerProfileId)
        {
            var profile = await _context.JobSeekerProfiles
                .FirstOrDefaultAsync(p =>
                    p.JobSeekerProfileId == jobSeekerProfileId);

            if (profile == null)
                return;

            _context.JobSeekerProfiles.Remove(profile);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByUserIdAsync(int userId)
        {
            return await _context.JobSeekerProfiles
                .AnyAsync(p => p.UserId == userId);
        }
    }
}