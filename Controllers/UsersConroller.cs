using ASP_Homework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lesson1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApiService _apiService;
        private readonly ILogger<UsersController> _logger;
        private static readonly HashSet<string> Usernames = new();

        public UsersController(ApiService apiService, ILogger<UsersController> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            if (Usernames.Contains(user.First_Name))
            {
                _logger.LogWarning("Duplicate username attempted: {Username}", user.First_Name);
                throw new DuplicateUserException("Username already exists.");
            }

            Usernames.Add(user.First_Name);

            var createdUser = await _apiService.CreateUserAsync(user);
            if (createdUser == null)
            {
                _logger.LogError("Failed to create user {Username}", user.First_Name);
                return BadRequest("Failed to create user.");
            }

            _logger.LogInformation("User {Username} created successfully.", user.First_Name);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _apiService.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", id);
                return NotFound();
            }
            _logger.LogInformation("Retrieved user {User}", user.First_Name);
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User updatedUser)
        {
            var updated = await _apiService.UpdateUserAsync(id, updatedUser);
            if (!updated)
            {
                _logger.LogWarning("User update failed for ID {UserId}", id);
                return NotFound("User not found for update");
            }

            _logger.LogInformation("User with ID {UserId} updated successfully.", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _apiService.DeleteUserAsync(id);
            if (!deleted)
            {
                _logger.LogWarning("User delete failed for ID {UserId}", id);
                return NotFound("User not found for deletion");
            }

            _logger.LogInformation("User with ID {UserId} deleted successfully.", id);
            return NoContent();
        }
    }
}