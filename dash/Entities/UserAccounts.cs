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

        public bool TrialCompleted { get; set; }

        public DateTime TrialStart { get; set; }

        public int TrialDaysLength { get; set; } = 7;
        
        public ICollection<UserRole>? UserRoles { get; set; }
        
        public ICollection<UserPermission>? UserPermissions { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}