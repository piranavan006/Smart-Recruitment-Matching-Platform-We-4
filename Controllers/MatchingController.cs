using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.API.Services.Interfaces;

namespace SmartRecruitment.API.Controllers
{
    [ApiController]
    [Route("api/matching")]
    [Authorize]
    public class MatchingController : ControllerBase
    {
        private readonly IMatchingService _matchingService;

        public MatchingController(
            IMatchingService matchingService)
        {
            _matchingService = matchingService;
        }

        // GET: api/matching/job/5
        [HttpGet("job/{jobId:int}")]
        public async Task<IActionResult> GetMatchesForJob(
            int jobId)
        {
            try
            {
                var result =
                    await _matchingService
                        .GetMatchesForJobAsync(jobId);

                return Ok(result);
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

        // GET: api/matching/jobseeker/5
        [HttpGet("jobseeker/{jobSeekerId:int}")]
        public async Task<IActionResult>
            GetMatchesForJobSeeker(
                int jobSeekerId)
        {
            try
            {
                var result =
                    await _matchingService
                        .GetMatchesForJobSeekerAsync(
                            jobSeekerId);

                return Ok(result);
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
    }
}