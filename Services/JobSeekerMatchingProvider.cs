using Microsoft.EntityFrameworkCore;
using SmartRecruitment.API.Data;
using SmartRecruitment.API.Models;
using SmartRecruitment.API.Services.Interfaces;
using System.Text.RegularExpressions;

namespace SmartRecruitment.API.Services
{
    public class JobSeekerMatchingProvider : IJobSeekerMatchingProvider
    {
        private readonly ApplicationDbContext _context;

        public JobSeekerMatchingProvider(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<JobSeekerMatchingProfile>> GetAllProfilesAsync()
        {
            var profiles = await _context.JobSeekerProfiles.ToListAsync();
            var userIds = profiles.Select(p => p.UserId).Distinct().ToList();
            var users = await _context.Users
                .Where(u => userIds.Contains(u.UserId))
                .ToDictionaryAsync(u => u.UserId);

            return profiles.Select(p => MapToMatchingProfile(p, users.GetValueOrDefault(p.UserId))).ToList();
        }

        public async Task<JobSeekerMatchingProfile?> GetProfileAsync(int jobSeekerId)
        {
            var profile = await _context.JobSeekerProfiles
                .FirstOrDefaultAsync(p =>
                    p.JobSeekerProfileId == jobSeekerId ||
                    p.UserId == jobSeekerId);

            if (profile == null)
            {
                return null;
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == profile.UserId);
            return MapToMatchingProfile(profile, user);
        }

        private static JobSeekerMatchingProfile MapToMatchingProfile(JobSeekerProfile profile, User? user)
        {
            var skills = (profile.Skills ?? string.Empty)
                .Split(new[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList();

            int experienceYears = 0;
            if (!string.IsNullOrWhiteSpace(profile.Experience))
            {
                var match = Regex.Match(profile.Experience, @"\d+");
                if (match.Success && int.TryParse(match.Value, out var years))
                {
                    experienceYears = years;
                }
            }

            return new JobSeekerMatchingProfile
            {
                JobSeekerId = profile.JobSeekerProfileId,
                FullName = user?.FullName ?? (!string.IsNullOrWhiteSpace(profile.Summary) ? profile.Summary : "Job Seeker"),
                Skills = skills,
                ExperienceYears = experienceYears,
                Education = profile.Education,
                Location = profile.Location
            };
        }
    }
}
