using ASP_Homework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lesson1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ApiService _apiService;

        public PostsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsByUserIdAndTitle([FromQuery] int userId, [FromQuery] string title)
        {
            var posts = await _apiService.GetPostsByUserIdAndTitleAsync(userId, title);
            if (!posts.Any()) return NoContent();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPostById(int id)
        {
            var post = await _apiService.GetPostByIdAsync(id);
            if (post == null) return NotFound();
            return Ok(post);
        }

        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost(Post post)
        {
            var createdPost = await _apiService.CreatePostAsync(post);
            if (createdPost == null) return BadRequest("Post could not be created");
            return CreatedAtAction(nameof(GetPostById), new { id = createdPost.Id }, createdPost);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var deleted = await _apiService.DeletePostAsync(id);
            if (!deleted) return NotFound("Post not found for deletion");
            return NoContent();
        }
    }
}
