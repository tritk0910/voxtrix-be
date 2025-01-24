using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Reaction
{
    [Key]
    public string ReactionId { get; set; }
    public string MessageId { get; set; }
    public Message Message { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }
    [StringLength(50)]
    public string Emoji { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}