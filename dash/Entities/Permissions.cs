using System.ComponentModel.DataAnnotations;

namespace dash.Entities
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}