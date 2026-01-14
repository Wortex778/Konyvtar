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
    public class BookController : ControllerBase
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;

        public BookController(LibraryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookReadDTO>> GetBook(int id)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();

            var bookDto = _mapper.Map<BookReadDTO>(book);
            return Ok(bookDto);
        }

        [HttpPost]
        public async Task<ActionResult<BookReadDTO>> CreateBook(BookCreateDTO bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var readDto = _mapper.Map<BookReadDTO>(book);
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, readDto);
        }
    }
}
