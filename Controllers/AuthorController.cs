using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Konyvtar.DTOs;
using Konyvtar.Data;
using Konyvtar.Models;

namespace Konyvtar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;

        public AuthorController(LibraryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorReadDTO>> GetAuthor(int id)
        {
            var author = await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null) return NotFound();

            return Ok(_mapper.Map<AuthorReadDTO>(author));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReadDTO>>> GetAllAuthors()
        {
            var authors = await _context.Authors.Include(a => a.Books).ToListAsync();
            return Ok(_mapper.Map<List<AuthorReadDTO>>(authors));
        }

        [HttpPost]
        public async Task<ActionResult<AuthorReadDTO>> CreateAuthor(AuthorCreateDTO authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, _mapper.Map<AuthorReadDTO>(author));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, AuthorCreateDTO authorDto)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return NotFound();

            _mapper.Map(authorDto, author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return NotFound();

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReadDTO>>> GetAuthors()
        {
            var authors = await _context.Authors.ToListAsync();
            var authorsDto = _mapper.Map<List<AuthorReadDTO>>(authors);
            return Ok(authorsDto);
        }
    }
}
