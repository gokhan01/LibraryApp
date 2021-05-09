using LibraryApp.Models.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApp.Models
{
    public class Book : BaseEntity
    {
        [CustomRequired, Column(TypeName = "nvarchar(255)"), Display(Name = "Kitap Adı")]
        public string Name { get; set; }

        [CustomRequired, Display(Name = "Adet")]
        public byte Quantity { get; set; }

        [CustomRequired, Display(Name = "Yazar")]
        public int AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public Author Author { get; set; }

        public ICollection<Member> Members { get; set; }
        public List<MemberBook> MemberBooks { get; set; }
    }
}
