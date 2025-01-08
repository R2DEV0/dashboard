using System.ComponentModel.DataAnnotations;

namespace dash.Models
{
    public class RegViewModel
    {
        [Required(ErrorMessage = "First Name is required")]
        [MinLength(2, ErrorMessage = "First Name must be at least 2 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [MinLength(2, ErrorMessage = "Last Name must be at least 2 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter valid email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "User Name must be between 3 to 20 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "Password must be at least 5 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Please confirm password")]
        public string ConfirmPassword { get; set; }
    }
}
