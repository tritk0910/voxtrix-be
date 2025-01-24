using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class UserRole
{
    [Key]
    public string UserRoleId { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public string RoleId { get; set; }
    public Role Role { get; set; }
}