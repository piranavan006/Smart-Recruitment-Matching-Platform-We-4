using SmartRecruitment.API.DTOs.Applications;

namespace SmartRecruitment.API.Services.Interfaces
{
    public interface IApplicationService
    {
        Task<ApplicationResponseDto?>
            GetByIdAsync(int applicationId);

        Task<List<ApplicationResponseDto>>
            GetMyApplicationsAsync(int jobSeekerId);

        Task<List<ApplicationResponseDto>>
            GetByJobAsync(int jobId);

        Task<ApplicationResponseDto>
            ApplyAsync(
                int jobSeekerId,
                ApplicationCreateDto dto);

        Task<ApplicationResponseDto?>
            UpdateStatusAsync(
                int applicationId,
                ApplicationStatusUpdateDto dto);
    }
}