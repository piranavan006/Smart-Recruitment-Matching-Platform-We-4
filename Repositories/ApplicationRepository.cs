using Microsoft.EntityFrameworkCore;
using SmartRecruitment.API.Data;
using SmartRecruitment.API.Models;
using SmartRecruitment.API.Repositories.Interfaces;

namespace SmartRecruitment.API.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Application?> GetByIdAsync(
            int applicationId)
        {
            return await _context.Applications
                .FirstOrDefaultAsync(
                    a => a.ApplicationId == applicationId);
        }

        public async Task<List<Application>>
            GetByJobSeekerIdAsync(int jobSeekerId)
        {
            return await _context.Applications
                .Include(a => a.Job)
                .Include(a => a.JobSeekerProfile)
                .Where(a => a.JobSeekerId == jobSeekerId ||
                            a.JobSeekerProfileId == jobSeekerId ||
                            (a.JobSeekerProfile != null && a.JobSeekerProfile.UserId == jobSeekerId))
                .OrderByDescending(a => a.AppliedAt)
                .ToListAsync();
        }

        public async Task<List<Application>>
            GetByJobIdAsync(int jobId)
        {
            return await _context.Applications
                .Include(a => a.JobSeekerProfile)
                    .ThenInclude(p => p != null ? p.User : null)
                .Where(a => a.JobId == jobId)
                .OrderByDescending(a => a.MatchScore)
                .ThenByDescending(a => a.AppliedAt)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(
            int jobSeekerId,
            int jobId)
        {
            return await _context.Applications
                .AnyAsync(a =>
                    (a.JobSeekerId == jobSeekerId ||
                     a.JobSeekerProfileId == jobSeekerId ||
                     (a.JobSeekerProfile != null && a.JobSeekerProfile.UserId == jobSeekerId)) &&
                    a.JobId == jobId);
        }

        public async Task<Application> AddAsync(
            Application application)
        {
            await _context.Applications
                .AddAsync(application);

            await _context.SaveChangesAsync();

            return application;
        }

        public async Task UpdateAsync(
            Application application)
        {
            _context.Applications.Update(application);

            await _context.SaveChangesAsync();
        }
    }
}