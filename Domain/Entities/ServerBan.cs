using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(ServerId), nameof(UserId), IsUnique = true)]
public class ServerBan
{
    [Key]
    public string BanId { get; set; } = Guid.NewGuid().ToString();
    public string ServerId { get; set; }
    public Server Server { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public string Reason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UnbannedAt { get; set; }
}