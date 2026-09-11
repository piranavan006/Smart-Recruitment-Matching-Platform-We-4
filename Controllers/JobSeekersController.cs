using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.API.DTOs.JobSeekers;
using SmartRecruitment.API.Services.Interfaces;
using System.Security.Claims;

namespace SmartRecruitment.API.Controllers
{
    [ApiController]
    [Route("api/jobseekers")]
    [Authorize]
    public class JobSeekersController : ControllerBase
    {
        private readonly IJobSeekerService _jobSeekerService;

        public JobSeekersController(
            IJobSeekerService jobSeekerService)
        {
            _jobSeekerService = jobSeekerService;
        }

        // GET: api/jobseekers/profile
        [HttpGet("profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized(new
                {
                    message = "User is not authenticated."
                });

            var profile =
                await _jobSeekerService.GetByUserIdAsync(userId.Value);

            if (profile == null)
                return NotFound(new
                {
                    message = "Job seeker profile not found."
                });

            return Ok(profile);
        }

        // GET: api/jobseekers/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var profile =
                await _jobSeekerService.GetByIdAsync(id);

            if (profile == null)
                return NotFound(new
                {
                    message = "Job seeker profile not found."
                });

            return Ok(profile);
        }

        // GET: api/jobseekers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var profiles =
                await _jobSeekerService.GetAllAsync();

            return Ok(profiles);
        }

        // POST: api/jobseekers/profile
        [HttpPost("profile")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> CreateProfile(
            [FromBody] CreateProfileDto dto)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized(new
                {
                    message = "User is not authenticated."
                });

            try
            {
                var profile =
                    await _jobSeekerService.CreateAsync(
                        userId.Value,
                        dto);

                return CreatedAtAction(
                    nameof(GetMyProfile),
                    null,
                    profile);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        // PUT: api/jobseekers/profile
        [HttpPut("profile")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> UpdateProfile(
            [FromBody] UpdateProfileDto dto)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized(new
                {
                    message = "User is not authenticated."
                });

            var profile =
                await _jobSeekerService.UpdateAsync(
                    userId.Value,
                    dto);

            if (profile == null)
                return NotFound(new
                {
                    message = "Job seeker profile not found."
                });

            return Ok(profile);
        }

        // DELETE: api/jobseekers/profile
        [HttpDelete("profile")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> DeleteProfile()
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized(new
                {
                    message = "User is not authenticated."
                });

            var deleted =
                await _jobSeekerService.DeleteByUserIdAsync(
                    userId.Value);

            if (!deleted)
                return NotFound(new
                {
                    message = "Job seeker profile not found."
                });

            return Ok(new
            {
                message = "Profile deleted successfully."
            });
        }

        private int? GetUserId()
        {
            var claim =
                User.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
                return null;

            return int.TryParse(
                claim.Value,
                out var userId)
                ? userId
                : null;
        }
    }
}