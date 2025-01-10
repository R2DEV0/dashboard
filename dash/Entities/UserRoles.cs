using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dash.Entities
{
    [Table("UserRoles")]
    public class UserRole
    {
        [Key, Column(Order = 0)]
        public string UserId { get; set; }

        [Key, Column(Order = 1)]
        public int RoleId { get; set; }

        public UserAccount User { get; set; }
        public Role Role { get; set; }
    }
}
