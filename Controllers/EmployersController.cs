using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.API.DTOs;
using SmartRecruitment.API.Services.Interfaces;

namespace SmartRecruitment.API.Controllers
{
    [ApiController]
    [Route("api/employers")]
    [Authorize(Roles = "Employer")]
    public class EmployersController : ControllerBase
    {
        private readonly IEmployerService _employerService;

        public EmployersController(IEmployerService employerService)
        {
            _employerService = employerService;
        }

        // POST: api/employers/profile
        [HttpPost("profile")]
        public async Task<IActionResult> CreateProfile(
            [FromBody] CreateEmployerProfileDto dto)
        {
            try
            {
                var userId = GetUserId();

                var result =
                    await _employerService.CreateProfileAsync(
                        userId, dto);

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // GET: api/employers/profile
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = GetUserId();

                var result =
                    await _employerService.GetProfileAsync(userId);

                if (result == null)
                {
                    return NotFound(new
                    {
                        message = "Employer profile not found."
                    });
                }

                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // PUT: api/employers/profile
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(
            [FromBody] UpdateEmployerProfileDto dto)
        {
            try
            {
                var userId = GetUserId();

                var result =
                    await _employerService.UpdateProfileAsync(
                        userId, dto);

                if (result == null)
                {
                    return NotFound(new
                    {
                        message = "Employer profile not found."
                    });
                }

                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        private string GetUserId()
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedAccessException(
                    "User ID not found in token.");
            }

            return userId;
        }
    }
}