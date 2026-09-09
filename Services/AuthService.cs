using SmartRecruitment.API.DTOs.Auth;
using SmartRecruitment.API.Helpers;
using SmartRecruitment.API.Models;
using SmartRecruitment.API.Repositories.Interfaces;
using SmartRecruitment.API.Services.Interfaces;

namespace SmartRecruitment.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtHelper _jwtHelper;

        public AuthService(
            IUserRepository userRepository,
            JwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
        }

        public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
        {
            // Check whether email already exists
            if (await _userRepository.ExistsByEmailAsync(dto.Email))
            {
                return null;
            }

            // Create new user
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = PasswordHasher.HashPassword(dto.Password),
                Role = dto.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _userRepository.AddAsync(user);

            // Generate JWT token
            var token = _jwtHelper.GenerateToken(createdUser);

            return new AuthResponseDto
            {
                UserId = createdUser.UserId,
                FullName = createdUser.FullName,
                Email = createdUser.Email,
                Role = createdUser.Role,
                Token = token
            };
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            // Find user by email
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null)
            {
                return null;
            }

            // Check active status
            if (!user.IsActive)
            {
                return null;
            }

            // Verify password
            if (!PasswordHasher.VerifyPassword(
                    dto.Password,
                    user.PasswordHash))
            {
                return null;
            }

            // Generate JWT token
            var token = _jwtHelper.GenerateToken(user);

            return new AuthResponseDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Token = token
            };
        }
    }
}