using System.Collections.Concurrent;
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

        // Temporary in-memory OTP storage
        private static readonly ConcurrentDictionary<string, OtpData> _otpStore
            = new();

        public AuthService(
            IUserRepository userRepository,
            JwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
        }

        // ==============================
        // REGISTER
        // ==============================
        public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
        {
            if (await _userRepository.ExistsByEmailAsync(dto.Email))
            {
                return null;
            }

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

        // ==============================
        // LOGIN
        // ==============================
        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null)
            {
                return null;
            }

            if (!user.IsActive)
            {
                return null;
            }

            if (!PasswordHasher.VerifyPassword(
                    dto.Password,
                    user.PasswordHash))
            {
                return null;
            }

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

        // ==============================
        // FORGOT PASSWORD
        // ==============================
        public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null)
            {
                return false;
            }

            if (!user.IsActive)
            {
                return false;
            }

            // Generate 6 digit OTP
            var otp = OtpGenerator.GenerateOtp();

            // OTP expires after 5 minutes
            var otpData = new OtpData
            {
                Otp = otp,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsVerified = false
            };

            _otpStore[dto.Email.ToLower()] = otpData;

            // Email sending will be connected later.
            // For now OTP is generated and stored securely in memory.
            Console.WriteLine(
                $"Password Reset OTP for {dto.Email}: {otp}");

            return true;
        }

        // ==============================
        // VERIFY OTP
        // ==============================
        public async Task<bool> VerifyOtpAsync(VerifyOtpDto dto)
        {
            var email = dto.Email.ToLower();

            if (!_otpStore.TryGetValue(email, out var otpData))
            {
                return false;
            }

            // Check OTP expiry
            if (DateTime.UtcNow > otpData.ExpiresAt)
            {
                _otpStore.TryRemove(email, out _);
                return false;
            }

            // Check OTP
            if (otpData.Otp != dto.Otp)
            {
                return false;
            }

            // OTP verified
            otpData.IsVerified = true;

            _otpStore[email] = otpData;

            return true;
        }

        // ==============================
        // RESET PASSWORD
        // ==============================
        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var email = dto.Email.ToLower();

            if (!_otpStore.TryGetValue(email, out var otpData))
            {
                return false;
            }

            // OTP must be verified first
            if (!otpData.IsVerified)
            {
                return false;
            }

            // Check OTP expiry
            if (DateTime.UtcNow > otpData.ExpiresAt)
            {
                _otpStore.TryRemove(email, out _);
                return false;
            }

            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null)
            {
                return false;
            }

            // Hash new password
            user.PasswordHash =
                PasswordHasher.HashPassword(dto.NewPassword);

            await _userRepository.UpdateAsync(user);

            // Remove OTP after successful password reset
            _otpStore.TryRemove(email, out _);

            return true;
        }

        // ==============================
        // OTP DATA CLASS
        // ==============================
        private class OtpData
        {
            public string Otp { get; set; } = string.Empty;

            public DateTime ExpiresAt { get; set; }

            public bool IsVerified { get; set; }
        }
    }
}