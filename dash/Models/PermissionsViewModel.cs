using dash.Entities;

namespace dash.Models
{
    public class PermissionsViewModel
    {
        public List<Permission> Permissions { get; set; }
        public List <Role> Roles { get; set; }
        public List <UserAccount> Users { get; set; }
    }
}
