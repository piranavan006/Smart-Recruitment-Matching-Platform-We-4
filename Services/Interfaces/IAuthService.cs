using SmartRecruitment.API.DTOs.Auth;

namespace SmartRecruitment.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> RegisterAsync(RegisterDto dto);

        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    }
}