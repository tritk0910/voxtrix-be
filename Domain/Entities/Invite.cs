using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(InviteCode), IsUnique = true)]
public class Invite
{
    [Key]
    public string InviteId { get; set; } = Guid.NewGuid().ToString();
    public string ServerId { get; set; }
    public Server Server { get; set; }
    [StringLength(10)]
    public string InviteCode { get; set; }
    public int? MaxUses { get; set; } // Changed to nullable int to allow unlimited uses
    public int Uses { get; set; } = 0;
    [ForeignKey("User")]
    public string AuthorId { get; set; }
    public bool IsPaused { get; set; } = false;
    public AppUser Author { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiredAt { get; set; }
}