using LibraryApp.Models.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class MemberBook : BaseEntity
    {
        [CustomRequired, Display(Name = "Üye")]
        public int MemberId { get; set; }
        public Member Member { get; set; }

        [CustomRequired, Display(Name = "Kitap")]
        public int BookId { get; set; }
        public Book Book { get; set; }

        [CustomRequired, Display(Name = "Veriliş Tarihi")]
        public DateTime DeliveryDate { get; set; }

        [Display(Name = "Teslim etti mi?")]
        public bool IsTakenBack { get; set; }
    }
}
