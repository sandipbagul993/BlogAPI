using AutoMapper;
using BlogAPI.DTO;
using BlogAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly BlogDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<PostsController> _logger;
        public PostsController(BlogDbContext context, IMapper mapper, ILogger<PostsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;   
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetPosts()
        {        
            try
            {
                var posts = await _context.Posts.ToListAsync();
                var postDTO = _mapper.Map<IEnumerable<PostDTO>>(posts);
                return Ok(postDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting posts");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDTO>> GetPost(int id)
        {           
            try
            {
                var post = await _context.Posts.FindAsync(id);
                if (post == null)
                {
                    _logger.LogWarning($"Post with ID {id} not found");
                    return NotFound("Post not found");
                }

                var postDTO = _mapper.Map<PostDTO>(post);
                return Ok(postDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting post with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/Posts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, PostDTO postDTO)
        {
            if (id != postDTO.Id)
            {
                return BadRequest("Post ID mismatch");
            }

            try
            {
                var post = await _context.Posts.FindAsync(id);
                if (post == null)
                {
                    return NotFound("Post not found");
                }

                _mapper.Map(postDTO, post);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, $"Concurrency error occurred while updating post with ID {id}");
                return StatusCode(500, "Concurrency error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating post with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/Posts
        [HttpPost]
        public async Task<ActionResult<PostDTO>> PostPost(PostDTO postDTO)
        {
            try
            {
                var post = _mapper.Map<Post>(postDTO);
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
                var createdPostDTO = _mapper.Map<PostDTO>(post);
                return CreatedAtAction(nameof(GetPost), new { id = post.Id }, createdPostDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating post");
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                var post = await _context.Posts.FindAsync(id);
                if (post == null)
                {
                    return NotFound("Post not found");
                }

                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting post with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

   
    }
}
