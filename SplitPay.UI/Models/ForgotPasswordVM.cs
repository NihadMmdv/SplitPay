using System.ComponentModel.DataAnnotations;

namespace SplitPay.UI.Models
{
    public class ForgotPasswordVM
    {
        [Required]
        public string Email { get; set; }
    }
}
