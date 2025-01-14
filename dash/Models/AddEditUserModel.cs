using dash.Entities;
using System.ComponentModel.DataAnnotations;

namespace dash.Models
{
    public class AddEditUserModel
    {
        public string? Id { get; set; }

        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Email { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        public ICollection<int>? SelectedRoleIds { get; set; }
        public ICollection<int>? SelectedPermissionIds { get; set; }

        public List<Role>? AllRoles { get; set; }
        public List<Permission>? AllPermissions { get; set; }

        // To track existing roles and permissions
        public ICollection<UserRole>? CurrentUserRoles { get; set; }
        public ICollection<UserPermission>? CurrentUserPermissions { get; set; }
    }
}
