using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApp.Models
{
    public abstract class Person : BaseEntity
    {
        [Required, Column(Order = 1, TypeName = "nvarchar(255)")]
        public string Name { get; set; }


        [Required, Column(Order = 2, TypeName = "nvarchar(255)")]
        public string Surname { get; set; }

        [NotMapped]
        public string FullName => $"{Name} {Surname}";
    }
}
