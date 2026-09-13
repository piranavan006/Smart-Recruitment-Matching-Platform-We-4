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

        // Get Job by ID
        public async Task<Job?> GetByIdAsync(int jobId)
        {
            return await _context.Jobs
                .Include(j => j.EmployerProfile)
                .Include(j => j.JobSkills)
                .FirstOrDefaultAsync(j => j.Id == jobId);
        }

        // Get All Jobs
        public async Task<List<Job>> GetAllAsync()
        {
            // Auto-close any dangling open jobs for deactivated employers
            var inactiveUserIds = await _context.Users
                .Where(u => !u.IsActive)
                .Select(u => u.UserId.ToString())
                .ToListAsync();

            if (inactiveUserIds.Any())
            {
                var danglingJobs = await _context.Jobs
                    .Include(j => j.EmployerProfile)
                    .Where(j => !j.IsClosed && j.EmployerProfile != null && inactiveUserIds.Contains(j.EmployerProfile.UserId))
                    .ToListAsync();

                if (danglingJobs.Any())
                {
                    foreach (var dj in danglingJobs)
                    {
                        dj.IsClosed = true;
                        dj.Status = "Closed";
                        dj.UpdatedAt = DateTime.UtcNow;
                    }
                    await _context.SaveChangesAsync();
                }
            }

            return await _context.Jobs
                .Include(j => j.EmployerProfile)
                .Include(j => j.JobSkills)
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();
        }

        // Get Jobs by Employer
        public async Task<List<Job>> GetByEmployerIdAsync(
            int employerProfileId)
        {
            return await _context.Jobs
                .Include(j => j.EmployerProfile)
                .Include(j => j.JobSkills)
                .Where(j =>
                    j.EmployerProfileId == employerProfileId)
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();
        }

        // Get Active Jobs
        public async Task<List<Job>> GetActiveJobsAsync()
        {
            var activeUserIds = await _context.Users
                .Where(u => u.IsActive)
                .Select(u => u.UserId.ToString())
                .ToListAsync();

            return await _context.Jobs
                .Include(j => j.EmployerProfile)
                .Include(j => j.JobSkills)
                .Where(j => !j.IsClosed && j.EmployerProfile != null && activeUserIds.Contains(j.EmployerProfile.UserId))
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();
        }

        // Search Jobs
        public async Task<List<Job>> SearchAsync(
            string? keyword,
            string? location,
            string? education,
            int? minExperienceYears,
            int? maxExperienceYears,
            decimal? salaryMin,
            decimal? salaryMax)
        {
            var activeUserIds = await _context.Users
                .Where(u => u.IsActive)
                .Select(u => u.UserId.ToString())
                .ToListAsync();

            var query = _context.Jobs
                .Include(j => j.EmployerProfile)
                .Include(j => j.JobSkills)
                .AsQueryable();

            // Active jobs only belonging to active employers
            query = query.Where(j => !j.IsClosed && j.EmployerProfile != null && activeUserIds.Contains(j.EmployerProfile.UserId));

            // Keyword search
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var search = keyword.Trim().ToLower();

                query = query.Where(j =>
                    j.Title.ToLower().Contains(search) ||
                    j.Description.ToLower().Contains(search));
            }

            // Location filter
            if (!string.IsNullOrWhiteSpace(location))
            {
                var search = location.Trim().ToLower();

                query = query.Where(j =>
                    j.Location != null &&
                    j.Location.ToLower().Contains(search));
            }

            // Education filter
            if (!string.IsNullOrWhiteSpace(education))
            {
                var search = education.Trim().ToLower();

                query = query.Where(j =>
                    j.Education != null &&
                    j.Education.ToLower().Contains(search));
            }

            // Minimum experience
            if (minExperienceYears.HasValue)
            {
                query = query.Where(j =>
                    j.MaxExperienceYears >=
                    minExperienceYears.Value);
            }

            // Maximum experience
            if (maxExperienceYears.HasValue)
            {
                query = query.Where(j =>
                    j.MinExperienceYears <=
                    maxExperienceYears.Value);
            }

            // Minimum salary
            if (salaryMin.HasValue)
            {
                query = query.Where(j =>
                    j.SalaryMax.HasValue &&
                    j.SalaryMax.Value >= salaryMin.Value);
            }

            // Maximum salary
            if (salaryMax.HasValue)
            {
                query = query.Where(j =>
                    j.SalaryMin.HasValue &&
                    j.SalaryMin.Value <= salaryMax.Value);
            }

            return await query
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();
        }

        // Add Job
        public async Task<Job> AddAsync(Job job)
        {
            await _context.Jobs.AddAsync(job);

            await _context.SaveChangesAsync();

            return job;
        }

        // Update Job
        public async Task<Job> UpdateAsync(Job job)
        {
            _context.Jobs.Update(job);

            await _context.SaveChangesAsync();

            return job;
        }

        // Delete Job
        public async Task<bool> DeleteAsync(int jobId)
        {
            var job = await _context.Jobs
                .Include(j => j.JobSkills)
                .FirstOrDefaultAsync(j => j.Id == jobId);

            if (job == null)
                return false;

            var applications = await _context.Applications
                .Where(a => a.JobId == jobId)
                .ToListAsync();

            if (applications.Any())
            {
                _context.Applications.RemoveRange(applications);
            }

            _context.Jobs.Remove(job);

            await _context.SaveChangesAsync();
            return true;
        }

        // Check Job Exists
        public async Task<bool> ExistsByIdAsync(int jobId)
        {
            return await _context.Jobs
                .AnyAsync(j => j.Id == jobId);
        }

        // Close Job
        public async Task<bool> CloseAsync(int jobId)
        {
            var job = await _context.Jobs
                .FirstOrDefaultAsync(j => j.Id == jobId);

            if (job == null)
                return false;

            job.IsClosed = true;
            job.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}