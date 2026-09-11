using SmartRecruitment.API.DTOs.JobSeekers;
using SmartRecruitment.API.Models;

namespace SmartRecruitment.API.Services.Interfaces
{
    public interface IJobSeekerService
    {
        // Get profile by Profile ID
        Task<JobSeekerProfile?> GetByIdAsync(
            int jobSeekerProfileId);

        // Get profile by User ID
        Task<JobSeekerProfile?> GetByUserIdAsync(
            int userId);

        // Get all job seeker profiles
        Task<List<JobSeekerProfile>> GetAllAsync();

        // Create job seeker profile
        Task<JobSeekerProfile> CreateAsync(
            int userId,
            CreateProfileDto dto);

        // Update job seeker profile
        Task<JobSeekerProfile?> UpdateAsync(
            int userId,
            UpdateProfileDto dto);

        // Delete profile by User ID
        Task<bool> DeleteByUserIdAsync(
            int userId);

        // Check whether profile exists for User
        Task<bool> ExistsByUserIdAsync(
            int userId);
    }
}
