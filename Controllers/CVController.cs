using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.API.DTOs.CV;
using SmartRecruitment.API.Services.Interfaces;
using System.Security.Claims;

namespace SmartRecruitment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CVController : ControllerBase
    {
        private readonly ICVService _cvService;

        public CVController(ICVService cvService)
        {
            _cvService = cvService;
        }

        // POST: api/cv/upload
        [HttpPost("upload")]
        [Authorize(Roles = "JobSeeker")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] UploadCvDto dto)
        {
            try
            {
                var userId = GetUserId();

                if (dto?.File == null || dto.File.Length == 0)
                {
                    return BadRequest(new { message = "CV file is required." });
                }

                var result = await _cvService.UploadAsync(userId, dto.File);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/cv
        [HttpGet]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> GetMyCv()
        {
            try
            {
                var userId = GetUserId();
                var cv = await _cvService.GetByUserIdAsync(userId);

                if (cv == null)
                {
                    return NotFound(new { message = "CV not found." });
                }

                return Ok(cv);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/cv
        [HttpDelete]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> DeleteCv()
        {
            try
            {
                var userId = GetUserId();
                var result = await _cvService.DeleteAsync(userId);

                if (!result)
                {
                    return NotFound(new { message = "CV not found." });
                }

                return Ok(new { message = "CV deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null || !int.TryParse(claim.Value, out var userId))
            {
                throw new UnauthorizedAccessException("User ID not found in token.");
            }
            return userId;
        }
    }
}
