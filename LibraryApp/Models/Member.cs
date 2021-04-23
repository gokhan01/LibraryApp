using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApp.Models
{
    public class Member : Person
    {
        [StringLength(11, MinimumLength = 11)]
        [Required, Column(TypeName = "char(11)"), DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required, Column(TypeName = "nvarchar(255)"), DataType(DataType.EmailAddress)]
        public string EMail { get; set; }

        [Required, Column(TypeName = "datetime2")]
        public DateTime RegisterDate { get; set; }

        public ICollection<Book> Books { get; set; }
        public List<MemberBook> MemberBooks { get; set; }
    }
}
