using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class AppUser : IdentityUser
{
    public string DisplayName { get; set; } = "";
    public string Address { get; set; } = "";
    public bool IsAdmin { get; set; } = false;
    public DateOnly DoB { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}