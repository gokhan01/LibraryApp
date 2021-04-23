using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
