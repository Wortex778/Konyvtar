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

            var authorDto = _mapper.Map<AuthorReadDTO>(author);
            return Ok(authorDto);
        }

        [HttpPost]
        public async Task<ActionResult<AuthorReadDTO>> CreateAuthor(AuthorCreateDTO authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            var readDto = _mapper.Map<AuthorReadDTO>(author);
            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, readDto);
        }
    }
}
