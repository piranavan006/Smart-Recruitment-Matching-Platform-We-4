using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.API.DTOs.Users;
using SmartRecruitment.API.Services.Interfaces;

namespace SmartRecruitment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Users/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound(new
                {
                    message = "User not found."
                });

            return Ok(user);
        }

        // PUT: api/Users/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(
            int id,
            [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedUser = await _userService.UpdateUserAsync(id, dto);

            if (updatedUser == null)
                return NotFound(new
                {
                    message = "User not found."
                });

            return Ok(updatedUser);
        }

        // DELETE: api/Users/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _userService.DeleteUserAsync(id);

            if (!deleted)
                return NotFound(new
                {
                    message = "User not found."
                });

            return Ok(new
            {
                message = "User deleted successfully."
            });
        }
    }
}