using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
