using AutoMapper;
using BlogAPI.DTO;
using BlogAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly BlogDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorsController> _logger;
        public AuthorsController(BlogDbContext context, IMapper mapper, ILogger<AuthorsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;   
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAuthors()
        {
            
            try
            {
                var author = await _context.Authors.ToListAsync();
                var authorDTO = _mapper.Map<IEnumerable<AuthorDTO>>(author);
                return Ok(authorDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting authors");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDTO>> GetAuthor(int id)
        {         
            try
            {
                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    _logger.LogWarning($"Author with ID {id} not found");
                    return NotFound("Author not found");
                }
                var authorDTO = _mapper.Map<AuthorDTO>(author);
                return Ok(authorDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting author with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/Authors/5 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorDTO authorDTO)
        {
          
            if (id != authorDTO.Id)
            {
                return BadRequest();
            }

            try
            {
                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    return NotFound("Author not found");
                }
                _mapper.Map(authorDTO, author);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, $"Concurrency error occurred while updating author with ID {id}");
                return StatusCode(500, "Concurrency error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating author with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/Authors
        [HttpPost]
        public async Task<ActionResult<AuthorDTO>> PostAuthor(AuthorDTO authorDTO)
        {
    
            try
            {
                var author = _mapper.Map<Author>(authorDTO);
                _context.Authors.Add(author);
                await _context.SaveChangesAsync();
                var createdAuthorDTO = _mapper.Map<AuthorDTO>(author);
                return CreatedAtAction("GetAuthor", new { id = createdAuthorDTO.Id }, createdAuthorDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Author");
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {         
            try
            {
                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    return NotFound("author not found");
                }
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting author with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

       
    }
}
