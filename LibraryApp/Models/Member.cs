using LibraryApp.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApp.Models
{
    public class Member : Person
    {
        [StringLength(11, MinimumLength = 11)]
        [CustomRequired, Column(TypeName = "char(11)"), DataType(DataType.PhoneNumber), Display(Name = "Telefon Numarası")]
        public string PhoneNumber { get; set; }

        [CustomRequired, EmailAddress, Column(TypeName = "nvarchar(255)"), DataType(DataType.EmailAddress), Display(Name = "E-Posta")]
        public string EMail { get; set; }

        [CustomRequired, Column(TypeName = "datetime2"), Display(Name = "Kayıt Tarihi")]
        public DateTime RegisterDate { get; set; }

        public ICollection<Book> Books { get; set; }
        public List<MemberBook> MemberBooks { get; set; }
    }
}
