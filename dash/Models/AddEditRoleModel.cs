using dash.Entities;

namespace dash.Models
{
    public class AddEditRoleModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public ICollection<int>? SelectedPermissionIds { get; set; }

        public List<Permission>? AllPermissions { get; set; }

        // To track existing roles and permissions
        public ICollection<RolePermission>? CurrentRolePermissions { get; set; }
    }
}
