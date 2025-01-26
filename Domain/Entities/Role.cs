using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Role
{
    [Key]
    public string RoleId { get; set; }
    [StringLength(100)]
    public string RoleName { get; set; }
    public string Permissions { get; set; }
    [StringLength(7)]
    public string Color { get; set; }
    public int Position { get; set; }
    public string ServerId { get; set; }
    public Server Server { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}