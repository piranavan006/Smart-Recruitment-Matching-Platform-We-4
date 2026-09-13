using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.API.DTOs;
using SmartRecruitment.API.Services.Interfaces;

namespace SmartRecruitment.API.Controllers
{
    [ApiController]
    [Route("api/jobs")]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobsController(IJobService jobService)
        {
            _jobService = jobService;
        }

        // POST: api/jobs
        [HttpPost]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> CreateJob(
            [FromBody] CreateJobDto dto)
        {
            try
            {
                var userId = GetUserId();

                var result =
                    await _jobService.CreateAsync(
                        userId, dto);

                return CreatedAtAction(
                    nameof(GetJobById),
                    new { id = result.Id },
                    result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new
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

        // GET: api/jobs/5
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetJobById(
            int id)
        {
            try
            {
                var result =
                    await _jobService.GetByIdAsync(id);

                if (result == null)
                {
                    return NotFound(new
                    {
                        message = "Job not found."
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // GET: api/jobs/mine
        [HttpGet("mine")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetMyJobs()
        {
            try
            {
                var userId = GetUserId();

                var result =
                    await _jobService.GetMyJobsAsync(userId);

                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
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

        // PUT: api/jobs/5
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> UpdateJob(
            int id,
            [FromBody] UpdateJobDto dto)
        {
            try
            {
                var userId = GetUserId();

                var result =
                    await _jobService.UpdateAsync(
                        userId, id, dto);

                if (result == null)
                {
                    return NotFound(new
                    {
                        message = "Job not found."
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
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
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

        // PUT: api/jobs/5/close
        [HttpPut("{id:int}/close")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> CloseJob(
            int id)
        {
            try
            {
                var userId = GetUserId();

                var result =
                    await _jobService.CloseAsync(
                        userId, id);

                if (!result)
                {
                    return NotFound(new
                    {
                        message = "Job not found."
                    });
                }

                return Ok(new
                {
                    message = "Job closed successfully."
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
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

        // GET: api/jobs/search
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchJobs(
            [FromQuery] JobSearchDto dto)
        {
            try
            {
                var result =
                    await _jobService.SearchAsync(dto);

                return Ok(result);
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