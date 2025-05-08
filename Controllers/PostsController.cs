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
    public class PostsController : ControllerBase
    {
        private readonly ApiService _apiService;
        private readonly ILogger<PostsController> _logger;

        public PostsController(ApiService apiService, ILogger<PostsController> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsByUserIdAndTitle([FromQuery] int userId, [FromQuery] string title)
        {
            var posts = await _apiService.GetPostsByUserIdAndTitleAsync(userId, title);
            if (!posts.Any())
            {
                _logger.LogInformation("No posts found for userId={UserId} with title containing '{Title}'", userId, title);
                return NoContent();
            }

            _logger.LogInformation("Returned {Count} posts for userId={UserId} and title='{Title}'", posts.Count(), userId, title);
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPostById(int id)
        {
            var post = await _apiService.GetPostByIdAsync(id);
            if (post == null)
            {
                _logger.LogWarning("Post with ID {PostId} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("Returned post with ID {PostId}", id);
            return Ok(post);
        }

        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost(Post post)
        {
            var createdPost = await _apiService.CreatePostAsync(post);
            if (createdPost == null)
            {
                _logger.LogError("Failed to create post for userId={UserId}", post.UserId);
                return BadRequest("Post could not be created");
            }

            _logger.LogInformation("Post created successfully with ID {PostId}", createdPost.Id);
            return CreatedAtAction(nameof(GetPostById), new { id = createdPost.Id }, createdPost);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var deleted = await _apiService.DeletePostAsync(id);
            if (!deleted)
            {
                _logger.LogWarning("Post delete failed for ID {PostId}", id);
                return NotFound("Post not found for deletion");
            }

            _logger.LogInformation("Post with ID {PostId} deleted successfully.", id);
            return NoContent();
        }
    }
}
