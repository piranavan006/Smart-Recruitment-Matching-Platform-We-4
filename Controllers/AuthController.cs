using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.API.DTOs.Auth;
using SmartRecruitment.API.Services.Interfaces;

namespace SmartRecruitment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // ==========================================
        // POST: api/Auth/register
        // ==========================================
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(dto);

            if (result == null)
            {
                return Conflict(new
                {
                    message = "Email already exists."
                });
            }

            return Ok(result);
        }

        // ==========================================
        // POST: api/Auth/login
        // ==========================================
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(dto);

            if (result == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            return Ok(result);
        }

        // ==========================================
        // POST: api/Auth/forgot-password
        // ==========================================
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(
            [FromBody] ForgotPasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authService.ForgotPasswordAsync(dto);

                if (!result)
                {
                    return NotFound(new
                    {
                        message = "User not found or account is inactive."
                    });
                }

                return Ok(new
                {
                    message = "OTP generated successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to send OTP email. Please verify your email settings or try again later.",
                    error = ex.Message
                });
            }
        }

        // ==========================================
        // POST: api/Auth/verify-otp
        // ==========================================
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(
            [FromBody] VerifyOtpDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.VerifyOtpAsync(dto);

            if (!result)
            {
                return BadRequest(new
                {
                    message = "Invalid or expired OTP."
                });
            }

            return Ok(new
            {
                message = "OTP verified successfully."
            });
        }

        // ==========================================
        // POST: api/Auth/reset-password
        // ==========================================
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(
            [FromBody] ResetPasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.ResetPasswordAsync(dto);

            if (!result)
            {
                return BadRequest(new
                {
                    message = "Invalid OTP, OTP expired, or OTP not verified."
                });
            }

            return Ok(new
            {
                message = "Password reset successfully."
            });
        }
    }
}