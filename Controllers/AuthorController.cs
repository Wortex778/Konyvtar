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
    public class AuthorsController : ControllerBase
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;

        public AuthorsController(LibraryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // 3.1 GET: /api/authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReadDTO>>> GetAuthors()
        {
            var authors = await _context.Authors
                .Include(a => a.Books) // kapcsolódó könyvek betöltése
                .ToListAsync();

            return Ok(_mapper.Map<List<AuthorReadDTO>>(authors));
        }

        // 3.2 GET: /api/authors/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorReadDTO>> GetAuthor(int id)
        {
            var author = await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null) return NotFound();

            return Ok(_mapper.Map<AuthorReadDTO>(author));
        }

        // 3.3 POST: /api/authors
        [HttpPost]
        public async Task<ActionResult<AuthorReadDTO>> CreateAuthor(AuthorCreateDTO authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            var authorReadDto = _mapper.Map<AuthorReadDTO>(author);

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, authorReadDto);
        }

        // 3.4 PUT: /api/authors/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, AuthorCreateDTO authorDto)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return NotFound();

            _mapper.Map(authorDto, author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 3.5 DELETE: /api/authors/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return NotFound();

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
