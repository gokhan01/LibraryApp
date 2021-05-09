using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models.Attributes
{
    public class CustomRequiredAttribute : RequiredAttribute
    {
        public CustomRequiredAttribute()
        {
            this.ErrorMessage = "{0} alani boş geçilemez";
        }
    }
}
