using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace dash.Entities
{
    public class UserAccount : IdentityUser
    {
        [StringLength(30)]
        public string FirstName { get; set; }

        [StringLength(30)]
        public string LastName { get; set; }
        
        public ICollection<UserRole>? UserRoles { get; set; }
        
        public ICollection<UserPermission>? UserPermissions { get; set; }
    }
}
