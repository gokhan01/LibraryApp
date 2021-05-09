using LibraryApp.Models.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApp.Models
{
    public abstract class Person : BaseEntity
    {
        [CustomRequired, Column(Order = 1, TypeName = "nvarchar(255)"), Display(Name = "Ad")]
        public string Name { get; set; }


        [CustomRequired, Column(Order = 2, TypeName = "nvarchar(255)"), Display(Name = "Soyad")]
        public string Surname { get; set; }

        [NotMapped, Display(Name = "Ad-Soyad")]
        public string FullName => $"{Name} {Surname}";
    }
}
