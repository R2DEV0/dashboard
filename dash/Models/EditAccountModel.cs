
using System.ComponentModel.DataAnnotations;

namespace dash.Models
{
    public class EditAccountModel
    {
        public string Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Email { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }
    }
}
