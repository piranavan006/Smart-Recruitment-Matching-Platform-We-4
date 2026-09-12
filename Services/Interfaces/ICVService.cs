using Microsoft.AspNetCore.Http;
using SmartRecruitment.API.DTOs.CV;

namespace SmartRecruitment.API.Services.Interfaces
{
    public interface ICVService
    {
        Task<CvResponseDto> UploadAsync(int userId, IFormFile file);

        Task<CvResponseDto?> GetByUserIdAsync(int userId);

        Task<bool> DeleteAsync(int userId);
    }
}
