using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Invite
{
    [Key]
    public string InviteId { get; set; } = Guid.NewGuid().ToString();
    public string ServerId { get; set; }
    public Server Server { get; set; }
    [StringLength(10)]
    public string InviteCode { get; set; } = Guid.NewGuid().ToString()[..10];
    public int? MaxUses { get; set; } // Changed to nullable int to allow unlimited uses
    public int Uses { get; set; } = 0;
    [ForeignKey("User")]
    public string CreatedBy { get; set; }
    public AppUser User { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiredAt { get; set; } = DateTime.UtcNow.AddDays(7);
}