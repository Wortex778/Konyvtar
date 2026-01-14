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
            var book = await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);
            if (book == null) return NotFound();

            return Ok(_mapper.Map<BookReadDTO>(book));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReadDTO>>> GetAllBooks()
        {
            var books = await _context.Books.Include(b => b.Author).ToListAsync();
            return Ok(_mapper.Map<List<BookReadDTO>>(books));
        }

        [HttpPost]
        public async Task<ActionResult<BookReadDTO>> CreateBook(BookCreateDTO bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, _mapper.Map<BookReadDTO>(book));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, BookCreateDTO bookDto)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            _mapper.Map(bookDto, book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/book/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
