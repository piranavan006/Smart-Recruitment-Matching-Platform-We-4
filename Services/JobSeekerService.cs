using SmartRecruitment.API.DTOs.JobSeekers;
using SmartRecruitment.API.Models;
using SmartRecruitment.API.Repositories.Interfaces;
using SmartRecruitment.API.Services.Interfaces;

namespace SmartRecruitment.API.Services
{
    public class JobSeekerService : IJobSeekerService
    {
        private readonly IJobSeekerRepository _repository;

        public JobSeekerService(IJobSeekerRepository repository)
        {
            _repository = repository;
        }

        // Get profile by ID
        public async Task<JobSeekerProfile?> GetByIdAsync(
            int jobSeekerProfileId)
        {
            return await _repository.GetByIdAsync(jobSeekerProfileId);
        }

        // Get profile by User ID
        public async Task<JobSeekerProfile?> GetByUserIdAsync(
            int userId)
        {
            return await _repository.GetByUserIdAsync(userId);
        }

        // Get all profiles
        public async Task<List<JobSeekerProfile>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        // Create profile
        public async Task<JobSeekerProfile> CreateAsync(
            int userId,
            CreateProfileDto dto)
        {
            if (await _repository.ExistsByUserIdAsync(userId))
            {
                throw new InvalidOperationException(
                    "Job seeker profile already exists.");
            }

            var profile = new JobSeekerProfile
            {
                UserId = userId,
                Summary = dto.Summary ?? string.Empty,
                Skills = dto.Skills ?? string.Empty,
                Experience = dto.Experience ?? string.Empty,
                Education = dto.Education ?? string.Empty,
                Location = dto.Location ?? string.Empty
            };

            return await _repository.AddAsync(profile);
        }

        // Update profile
        public async Task<JobSeekerProfile?> UpdateAsync(
            int userId,
            UpdateProfileDto dto)
        {
            var profile =
                await _repository.GetByUserIdAsync(userId);

            if (profile == null)
            {
                return null;
            }

            profile.Summary = dto.Summary ?? string.Empty;
            profile.Skills = dto.Skills ?? string.Empty;
            profile.Experience = dto.Experience ?? string.Empty;
            profile.Education = dto.Education ?? string.Empty;
            profile.Location = dto.Location ?? string.Empty;

            await _repository.UpdateAsync(profile);

            return profile;
        }

        // Delete profile
        public async Task<bool> DeleteByUserIdAsync(
            int userId)
        {
            var profile =
                await _repository.GetByUserIdAsync(userId);

            if (profile == null)
            {
                return false;
            }

            await _repository.DeleteAsync(
                profile.JobSeekerProfileId);

            return true;
        }

        // Check profile exists
        public async Task<bool> ExistsByUserIdAsync(
            int userId)
        {
            return await _repository
                .ExistsByUserIdAsync(userId);
        }
    }
}