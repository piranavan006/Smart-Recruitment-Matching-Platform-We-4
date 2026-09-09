using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.API.Models;
using SmartRecruitment.API.Services.Interfaces;

namespace SmartRecruitment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // GET: api/Notifications
        [HttpGet]
        public async Task<ActionResult<List<Notification>>> GetAll()
        {
            var notifications = await _notificationService.GetAllAsync();

            return Ok(notifications);
        }

        // GET: api/Notifications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetById(int id)
        {
            var notification = await _notificationService.GetByIdAsync(id);

            if (notification == null)
            {
                return NotFound(new
                {
                    message = "Notification not found."
                });
            }

            return Ok(notification);
        }

        // GET: api/Notifications/user/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<Notification>>> GetByUserId(int userId)
        {
            var notifications = await _notificationService.GetByUserIdAsync(userId);

            return Ok(notifications);
        }

        // POST: api/Notifications
        [HttpPost]
        public async Task<ActionResult<Notification>> Create(Notification notification)
        {
            if (notification == null)
            {
                return BadRequest(new
                {
                    message = "Notification data is required."
                });
            }

            var createdNotification =
                await _notificationService.AddAsync(notification);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdNotification.NotificationId },
                createdNotification);
        }

        // PUT: api/Notifications/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            Notification notification)
        {
            if (id != notification.NotificationId)
            {
                return BadRequest(new
                {
                    message = "Notification ID mismatch."
                });
            }

            var exists = await _notificationService.ExistsByIdAsync(id);

            if (!exists)
            {
                return NotFound(new
                {
                    message = "Notification not found."
                });
            }

            await _notificationService.UpdateAsync(notification);

            return NoContent();
        }

        // DELETE: api/Notifications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await _notificationService.ExistsByIdAsync(id);

            if (!exists)
            {
                return NotFound(new
                {
                    message = "Notification not found."
                });
            }

            await _notificationService.DeleteAsync(id);

            return NoContent();
        }
    }
}