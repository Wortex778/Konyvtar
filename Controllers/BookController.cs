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
    public class BooksController : ControllerBase
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;

        public BooksController(LibraryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReadDTO>>> GetBooks()
        {
            var books = await _context.Books
                .Include(b => b.Author) 
                .ToListAsync();

            return Ok(_mapper.Map<List<BookReadDTO>>(books));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookReadDTO>> GetBook(int id)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();

            return Ok(_mapper.Map<BookReadDTO>(book));
        }

        [HttpPost]
        public async Task<ActionResult<BookReadDTO>> CreateBook(BookCreateDTO bookDto)
        {
            var author = await _context.Authors.FindAsync(bookDto.AuthorId);
            if (author == null)
                return BadRequest($"A megadott szerző ({bookDto.AuthorId}) nem található.");

            var book = _mapper.Map<Book>(bookDto);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var bookReadDto = _mapper.Map<BookReadDTO>(book);
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, bookReadDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, BookCreateDTO bookDto)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            var author = await _context.Authors.FindAsync(bookDto.AuthorId);
            if (author == null)
                return BadRequest($"A megadott szerző ({bookDto.AuthorId}) nem található.");

            _mapper.Map(bookDto, book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

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
