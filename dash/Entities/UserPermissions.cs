using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dash.Entities
{
    [Table("UserPermissions")]
    public class UserPermission
    {
        [Key, Column(Order = 0)]
        public string UserId { get; set; }

        [Key, Column(Order = 1)]
        public int PermissionId { get; set; }

        public UserAccount User { get; set; }
        public Permission Permission { get; set; }
    }
}
