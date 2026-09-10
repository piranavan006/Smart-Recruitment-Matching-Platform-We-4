using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.API.DTOs.Admin;
using SmartRecruitment.API.DTOs.Users;
using SmartRecruitment.API.Services.Interfaces;

namespace SmartRecruitment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Admin/dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var users = await _userService.GetAllUsersAsync();

            var dashboard = new AdminDashboardDto
            {
                TotalUsers = users.Count,

                TotalJobSeekers = users.Count(u =>
                    u.Role.Equals(
                        "JobSeeker",
                        StringComparison.OrdinalIgnoreCase)),

                TotalEmployers = users.Count(u =>
                    u.Role.Equals(
                        "Employer",
                        StringComparison.OrdinalIgnoreCase)),

                ActiveUsers = users.Count(u => u.IsActive),

                InactiveUsers = users.Count(u => !u.IsActive),

                // Jobs count will be connected later
                TotalJobs = 0
            };

            return Ok(dashboard);
        }

        // PUT: api/Admin/users/{id}/status
        [HttpPut("users/{id}/status")]
        public async Task<IActionResult> UpdateUserStatus(
            int id,
            [FromBody] UpdateUserStatusDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.UpdateUserStatusAsync(
                id,
                dto.IsActive);

            if (!result)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }

            return Ok(new
            {
                message = dto.IsActive
                    ? "User activated successfully."
                    : "User deactivated successfully.",

                userId = id,

                isActive = dto.IsActive
            });
        }
    }
}