using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
