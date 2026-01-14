namespace Konyvtar.DTOs
{
    public class BookCreateDTO
    {
        public string Title { get; set; } = null!;
        public int Year { get; set; }
        public int AuthorId { get; set; }
    }
}
