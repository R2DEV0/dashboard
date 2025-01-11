using dash.Entities;
using System.ComponentModel.DataAnnotations;

namespace dash.Models
{
    public class AddUserModel
    {
        public string? Id { get; set; }

        [StringLength(30)]
        public string FirstName { get; set; }

        [StringLength(30)]
        public string LastName { get; set; }

        [StringLength(50)]
        //[RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please Enter Valid Email.")]
        public string Email { get; set; }

        [StringLength(30)]
        public string UserName { get; set; }

        public ICollection<UserRole>? UserRoles { get; set; }

        public ICollection<UserPermission>? UserPermissions { get; set; }
    }
}
