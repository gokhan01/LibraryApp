using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class MemberBook : BaseEntity
    {
        [Required, Display(Name = "Üye")]
        public int MemberId { get; set; }
        public Member Member { get; set; }

        [Required, Display(Name = "Kitap")]
        public int BookId { get; set; }
        public Book Book { get; set; }

        [Required]
        public DateTime DeliveryDate { get; set; }

        public bool IsTakenBack { get; set; }
    }
}
