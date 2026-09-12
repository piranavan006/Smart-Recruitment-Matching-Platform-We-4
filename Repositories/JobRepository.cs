using Microsoft.EntityFrameworkCore;
using SmartRecruitment.API.Data;
using SmartRecruitment.API.Models;
using SmartRecruitment.API.Repositories.Interfaces;

namespace SmartRecruitment.API.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly ApplicationDbContext _context;

        public JobRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Job?> GetByIdAsync(
            int id)
        {
            return await _context.Jobs
                .Include(j => j.EmployerProfile)
                .Include(j => j.RequiredSkills)
                .FirstOrDefaultAsync(j => j.Id == id);
        }

        public async Task<List<Job>> GetByEmployerIdAsync(
            int employerProfileId)
        {
            return await _context.Jobs
                .Include(j => j.EmployerProfile)
                .Include(j => j.RequiredSkills)
                .Where(j =>
                    j.EmployerProfileId ==
                    employerProfileId)
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();
        }

        public async Task<Job> CreateAsync(
            Job job)
        {
            _context.Jobs.Add(job);

            await _context.SaveChangesAsync();

            return await _context.Jobs
                .Include(j => j.EmployerProfile)
                .Include(j => j.RequiredSkills)
                .FirstAsync(j => j.Id == job.Id);
        }

        public async Task<Job> UpdateAsync(
            Job job)
        {
            _context.Jobs.Update(job);

            await _context.SaveChangesAsync();

            return await _context.Jobs
                .Include(j => j.EmployerProfile)
                .Include(j => j.RequiredSkills)
                .FirstAsync(j => j.Id == job.Id);
        }

        public async Task<bool> CloseAsync(
            int id)
        {
            var job = await _context.Jobs
                .FirstOrDefaultAsync(j => j.Id == id);

            if (job == null)
                return false;

            job.IsClosed = true;
            job.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Job>> SearchAsync(
            string? keyword,
            string? location,
            string? education,
            int? minExperienceYears,
            int? maxExperienceYears,
            decimal? salaryMin,
            decimal? salaryMax)
        {
            var query = _context.Jobs
                .Include(j => j.EmployerProfile)
                .Include(j => j.RequiredSkills)
                .AsQueryable();

            // Only active jobs
            query = query.Where(j => !j.IsClosed);

            // Keyword search
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var search =
                    keyword.Trim().ToLower();

                query = query.Where(j =>
                    j.Title.ToLower()
                        .Contains(search)
                    ||
                    j.Description.ToLower()
                        .Contains(search));
            }

            // Location
            if (!string.IsNullOrWhiteSpace(location))
            {
                var search =
                    location.Trim().ToLower();

                query = query.Where(j =>
                    j.Location != null &&
                    j.Location.ToLower()
                        .Contains(search));
            }

            // Education
            if (!string.IsNullOrWhiteSpace(education))
            {
                var search =
                    education.Trim().ToLower();

                query = query.Where(j =>
                    j.Education != null &&
                    j.Education.ToLower()
                        .Contains(search));
            }

            // Minimum experience filter
            if (minExperienceYears.HasValue)
            {
                query = query.Where(j =>
                    j.MaxExperienceYears >=
                    minExperienceYears.Value);
            }

            // Maximum experience filter
            if (maxExperienceYears.HasValue)
            {
                query = query.Where(j =>
                    j.MinExperienceYears <=
                    maxExperienceYears.Value);
            }

            // Minimum salary filter
            if (salaryMin.HasValue)
            {
                query = query.Where(j =>
                    j.SalaryMax.HasValue &&
                    j.SalaryMax.Value >=
                    salaryMin.Value);
            }

            // Maximum salary filter
            if (salaryMax.HasValue)
            {
                query = query.Where(j =>
                    j.SalaryMin.HasValue &&
                    j.SalaryMin.Value <=
                    salaryMax.Value);
            }

            return await query
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();
        }
    }
}