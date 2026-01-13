namespace Konyvtar.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }

        // Foreign key
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}

