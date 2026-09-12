using SmartRecruitment.API.DTOs.Users;

namespace SmartRecruitment.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto?> GetUserByIdAsync(int userId);

        Task<List<UserResponseDto>> GetAllUsersAsync();

        Task<UserResponseDto?> UpdateUserAsync(
            int userId,
            UpdateUserDto dto);

        Task<bool> UpdateUserStatusAsync(
            int userId,
            bool isActive);

        Task<bool> DeleteUserAsync(int userId);
    }
}