using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Message
{
    [Key]
    public string MessageId { get; set; }
    public string Content { get; set; }
    public string AuthorId { get; set; }
    public AppUser User { get; set; }
    public string ChannelId { get; set; }
    public Channel Channel { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime EditedAt { get; set; }
    public string AttachmentURL { get; set; }
    public ICollection<Reaction> Reactions { get; set; } = [];
}