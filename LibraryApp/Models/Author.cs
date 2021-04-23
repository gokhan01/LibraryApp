using System.Collections.Generic;

namespace LibraryApp.Models
{
    public class Author : Person
    {
        public ICollection<Book> Books { get; set; }
    }
}
