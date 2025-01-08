using System.ComponentModel.DataAnnotations;

namespace dash.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "User Name or Email is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
