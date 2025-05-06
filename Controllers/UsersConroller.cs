using ASP_Homework;
using Microsoft.AspNetCore.Mvc;
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

        private static readonly HashSet<string> Usernames = new();

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            if (Usernames.Contains(user.First_Name))
                throw new DuplicateUserException("Username already exists.");

            Usernames.Add(user.First_Name);

            var createdUser = await _apiService.CreateUserAsync(user);
            Console.WriteLine($"User {user.First_Name} created successfully.");
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }
        public UsersController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _apiService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User updatedUser)
        {
            var updated = await _apiService.UpdateUserAsync(id, updatedUser);
            if (!updated) return NotFound("User not found for update");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _apiService.DeleteUserAsync(id);
            if (!deleted) return NotFound("User not found for deletion");
            return NoContent();
        }
    }
}
