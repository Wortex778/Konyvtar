using System.Collections.Generic;

namespace Konyvtar.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // 1 szerzőnek több könyve lehet
        public ICollection<Book> Books { get; set; }
    }
}
