using SmartRecruitment.API.DTOs;

namespace SmartRecruitment.API.Services.Interfaces
{
    public interface IEmployerService
    {
        Task<EmployerResponseDto> CreateProfileAsync(
            string userId,
            CreateEmployerProfileDto dto);

        Task<EmployerResponseDto?> GetProfileAsync(
            string userId);

        Task<EmployerResponseDto?> UpdateProfileAsync(
            string userId,
            UpdateEmployerProfileDto dto);

        Task<List<EmployerResponseDto>> GetAllAsync();

        Task<EmployerResponseDto?> SetApprovalAsync(
            int employerProfileId,
            bool isApproved,
            string? status = null);
    }
}