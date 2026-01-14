namespace Konyvtar.DTOs
{
    public class AuthorReadDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public List<BookReadDTO> Books { get; set; } = new();
    }
}
