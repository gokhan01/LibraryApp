using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApp.Models
{
    public class Book : BaseEntity
    {
        [Required, Column(TypeName = "nvarchar(255)")]
        public string Name { get; set; }
        [Required]
        public byte Quantity { get; set; }

        public int AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public Author Author { get; set; }

        public ICollection<Member> Members { get; set; }
        public List<MemberBook> MemberBooks { get; set; }
    }
}
