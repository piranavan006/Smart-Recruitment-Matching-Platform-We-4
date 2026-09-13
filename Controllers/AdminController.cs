using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.API.DTOs.Admin;
using SmartRecruitment.API.DTOs.Users;
using SmartRecruitment.API.Services.Interfaces;

namespace SmartRecruitment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator,Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmployerService _employerService;
        private readonly INotificationService _notificationService;

        public AdminController(
            IUserService userService,
            IEmployerService employerService,
            INotificationService notificationService)
        {
            _userService = userService;
            _employerService = employerService;
            _notificationService = notificationService;
        }

        // GET: api/Admin/users
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: api/Admin/employers
        [HttpGet("employers")]
        public async Task<IActionResult> GetEmployers()
        {
            var employers = await _employerService.GetAllAsync();
            return Ok(employers);
        }

        // PUT: api/Admin/employers/{id}/approval
        [HttpPut("employers/{id:int}/approval")]
        public async Task<IActionResult> UpdateEmployerApproval(
            int id,
            [FromBody] UpdateApprovalDto dto)
        {
            string status = !string.IsNullOrWhiteSpace(dto.Status)
                ? dto.Status
                : (dto.IsApproved ? "Approved" : "Pending");

            bool isApproved = status.Equals("Approved", StringComparison.OrdinalIgnoreCase);

            var updated = await _employerService.SetApprovalAsync(id, isApproved, status);
            if (updated == null)
            {
                return NotFound(new
                {
                    message = "Employer profile not found."
                });
            }

            if (int.TryParse(updated.UserId, out int userId))
            {
                try
                {
                    string notificationMsg;
                    if (isApproved)
                    {
                        notificationMsg = $"Congratulations! Your company profile '{updated.CompanyName}' has been approved by the platform administrator. You can now post job vacancies and reach out to candidates.";
                    }
                    else if (status.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
                    {
                        string reasonNote = !string.IsNullOrWhiteSpace(dto.Reason)
                            ? $"Reason: {dto.Reason}"
                            : "Profile details do not meet current platform verification criteria. Please update your profile information.";
                        notificationMsg = $"Your company profile '{updated.CompanyName}' verification was not approved. {reasonNote}";
                    }
                    else
                    {
                        notificationMsg = $"Your company profile '{updated.CompanyName}' verification status has been changed to: {status}.";
                    }

                    await _notificationService.AddAsync(new Models.Notification
                    {
                        UserId = userId,
                        Message = notificationMsg,
                        IsRead = false,
                        CreatedAt = DateTime.UtcNow
                    });
                }
                catch { }
            }

            return Ok(new
            {
                message = isApproved
                    ? "Employer profile approved successfully."
                    : (status.Equals("Rejected", StringComparison.OrdinalIgnoreCase)
                        ? "Employer profile rejected."
                        : "Employer profile approval revoked."),
                employer = updated
            });
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

        // DELETE: api/Admin/users/{id}
        [HttpDelete("users/{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var deleted = await _userService.DeleteUserAsync(id);
                if (!deleted)
                {
                    return NotFound(new
                    {
                        message = "User not found."
                    });
                }

                return Ok(new
                {
                    message = "User deleted successfully.",
                    userId = id
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to delete user: " + ex.Message
                });
            }
        }
    }
}