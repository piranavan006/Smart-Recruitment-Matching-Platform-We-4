using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.API.DTOs.Applications;
using SmartRecruitment.API.Services.Interfaces;
using System.Security.Claims;

namespace SmartRecruitment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationService _service;

        public ApplicationsController(
            IApplicationService service)
        {
            _service = service;
        }

        // Job Seeker applies for a job
        [HttpPost]
        [Authorize(Roles = "JobSeeker,Seeker,jobseeker,Jobseeker")]
        public async Task<IActionResult> Apply(
            ApplicationCreateDto dto)
        {
            try
            {
                int jobSeekerId = GetUserId();

                var result =
                    await _service.ApplyAsync(
                        jobSeekerId,
                        dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new
                    {
                        id = result.ApplicationId
                    },
                    result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
            catch (ArgumentException ex)
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
                    message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        // Job seeker views own applications
        [HttpGet("my")]
        [Authorize(Roles = "JobSeeker,Seeker,jobseeker,Jobseeker")]
        public async Task<IActionResult>
            GetMyApplications()
        {
            int jobSeekerId = GetUserId();

            var result =
                await _service
                    .GetMyApplicationsAsync(
                        jobSeekerId);

            return Ok(result);
        }

        // Employer views applications for a job
        [HttpGet("job/{jobId:int}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult>
            GetByJob(int jobId)
        {
            var result =
                await _service.GetByJobAsync(jobId);

            return Ok(result);
        }

        // Get one application
        [HttpGet("{id:int}")]
        public async Task<IActionResult>
            GetById(int id)
        {
            var result =
                await _service.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Application not found."
                });
            }

            return Ok(result);
        }

        // Employer updates application status
        [HttpPut("{id:int}/status")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult>
            UpdateStatus(
                int id,
                ApplicationStatusUpdateDto dto)
        {
            try
            {
                var result =
                    await _service.UpdateStatusAsync(
                        id,
                        dto);

                if (result == null)
                {
                    return NotFound(new
                    {
                        message =
                            "Application not found."
                    });
                }

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        private int GetUserId()
        {
            string? userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (!int.TryParse(
                userId,
                out int id))
            {
                throw new UnauthorizedAccessException(
                    "User ID not found in token.");
            }

            return id;
        }
    }
}