using System.ComponentModel.DataAnnotations;

namespace dash.Entities
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}