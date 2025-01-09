using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace dash.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "User Name or Email is required")]
        [DisplayName("Username or Email")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
