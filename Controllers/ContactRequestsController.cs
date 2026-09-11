using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.API.DTOs.ContactRequests;
using SmartRecruitment.API.Services.Interfaces;
using System.Security.Claims;

namespace SmartRecruitment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContactRequestsController
        : ControllerBase
    {
        private readonly IContactRequestService _service;

        public ContactRequestsController(
            IContactRequestService service)
        {
            _service = service;
        }

        // Employer sends contact request
        [HttpPost]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> Create(
            ContactRequestCreateDto dto)
        {
            try
            {
                int senderId = GetUserId();

                var result =
                    await _service.CreateAsync(
                        senderId,
                        dto);

                return CreatedAtAction(
                    nameof(GetReceived),
                    null,
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
        }

        // Employer views sent requests
        [HttpGet("sent")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetSent()
        {
            int senderId = GetUserId();

            var result =
                await _service.GetSentAsync(
                    senderId);

            return Ok(result);
        }

        // Job seeker views received requests
        [HttpGet("received")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> GetReceived()
        {
            int receiverId = GetUserId();

            var result =
                await _service.GetReceivedAsync(
                    receiverId);

            return Ok(result);
        }

        // Job seeker accepts / declines
        [HttpPut("{id:int}/status")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> Respond(
            int id,
            ContactRequestStatusUpdateDto dto)
        {
            try
            {
                int receiverId = GetUserId();

                var result =
                    await _service.RespondAsync(
                        id,
                        receiverId,
                        dto);

                if (result == null)
                {
                    return NotFound(new
                    {
                        message =
                            "Contact request not found."
                    });
                }

                return Ok(result);
            }
            catch (UnauthorizedAccessException )
            {
                return Forbid();
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
        }

        private int GetUserId()
        {
            string? value =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (!int.TryParse(value, out int id))
            {
                throw new UnauthorizedAccessException(
                    "User ID not found in token.");
            }

            return id;
        }
    }
}