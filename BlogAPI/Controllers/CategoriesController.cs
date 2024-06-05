using AutoMapper;
using BlogAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly BlogDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(BlogDbContext context, IMapper mapper, ILogger<CategoriesController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {

            try
            {
                var category = await _context.Categories.ToListAsync();
                var CategoryDTO = _mapper.Map<IEnumerable<CategoryDTO>>(category);
                return Ok(CategoryDTO);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting posts");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);

                if (category == null)
                {
                    _logger.LogWarning($"Category with ID {id} not found");
                    return NotFound("Post not found");
                }
                var CategoryDTO = _mapper.Map<CategoryDTO>(category);
                return Ok(CategoryDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting category with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/Categories/5

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, CategoryDTO categoryDTO)
        {

            if (id != categoryDTO.Id)
            {
                return BadRequest();
            }
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    return NotFound("Category not found");
                }
                _mapper.Map(categoryDTO, category);
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

        // POST: api/Categories
     
        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> PostCategory(CategoryDTO categoryDTO)
        { 
            try
            {
                var category = _mapper.Map<Category>(categoryDTO);
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                var createdCategoryDTO = _mapper.Map<CategoryDTO>(category);
                return CreatedAtAction("GetCategory", new { id = createdCategoryDTO.Id }, createdCategoryDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating category");
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {     
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    return NotFound("category not found");
                }
                _context.Categories.Remove(category);
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
