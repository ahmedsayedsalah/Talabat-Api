using System.ComponentModel.DataAnnotations;

namespace Talabat.Core.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$",ErrorMessage = "Email must be a valid format (e.g., user@example.com).")]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{6,}$",ErrorMessage =
			"Password must be at least 6 characters long and contain at least one uppercase letter, one lowercase letter, and one number.")]
        public string Password { get; set; }
        [Required]
        [RegularExpression("^(\\+201|01|00201)[0-2,5]{1}[0-9]{8}",ErrorMessage = 
            "Phone number must be a valid Egyptian mobile number starting with 010, 011, 012, or 015")]
        public string PhoneNumber { get; set; }
    }
}
