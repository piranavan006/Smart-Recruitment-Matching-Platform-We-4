using SmartRecruitment.API.DTOs;

namespace SmartRecruitment.API.Services.Interfaces
{
    public interface IJobService
    {
        Task<JobResponseDto> CreateAsync(
            string userId,
            CreateJobDto dto);

        Task<JobResponseDto?> GetByIdAsync(
            int id);

        Task<List<JobResponseDto>> GetMyJobsAsync(
            string userId);

        Task<JobResponseDto?> UpdateAsync(
            string userId,
            int id,
            UpdateJobDto dto);

        Task<bool> CloseAsync(
            string userId,
            int id);

        Task<List<JobResponseDto>> SearchAsync(
            JobSearchDto dto);

        Task<List<JobResponseDto>> GetAllAsync();

        Task<bool> DeleteAsync(
            int id,
            string? userId = null,
            bool isAdmin = false);
    }
}