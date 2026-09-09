using Microsoft.EntityFrameworkCore;
using SmartRecruitment.API.Data;
using SmartRecruitment.API.Models;
using SmartRecruitment.API.Repositories.Interfaces;

namespace SmartRecruitment.API.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly ApplicationDbContext _context;

        public JobRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Job?> GetByIdAsync(int jobId)
        {
            return await _context.Jobs
                .FirstOrDefaultAsync(j => j.JobId == jobId);
        }

        public async Task<List<Job>> GetAllAsync()
        {
            return await _context.Jobs
                .ToListAsync();
        }

        public async Task<List<Job>> GetByEmployerIdAsync(int employerProfileId)
        {
            return await _context.Jobs
                .Where(j => j.EmployerProfileId == employerProfileId)
                .ToListAsync();
        }

        public async Task<List<Job>> GetActiveJobsAsync()
        {
            return await _context.Jobs
                .Where(j => j.Status == "Open")
                .ToListAsync();
        }

        public async Task<Job> AddAsync(Job job)
        {
            await _context.Jobs.AddAsync(job);
            await _context.SaveChangesAsync();

            return job;
        }

        public async Task UpdateAsync(Job job)
        {
            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int jobId)
        {
            var job = await _context.Jobs
                .FirstOrDefaultAsync(j => j.JobId == jobId);

            if (job == null)
                return;

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(int jobId)
        {
            return await _context.Jobs
                .AnyAsync(j => j.JobId == jobId);
        }
    }
}