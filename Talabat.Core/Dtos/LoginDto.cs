using System.ComponentModel.DataAnnotations;

namespace Talabat.Core.Dtos
{
    public class LoginDto
    {
        [Required]
		[RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Invalid email address.")]
		public string Email { get; set; }
        [Required]
        [MinLength(6,ErrorMessage= "Password must be at least 6 characters")]
        public string Password { get; set; }
    }
}
