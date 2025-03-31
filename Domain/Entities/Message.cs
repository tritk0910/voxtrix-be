using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Message
{
    [Key]
    public string MessageId { get; set; } = Guid.NewGuid().ToString();
    public string Content { get; set; }
    public string AuthorId { get; set; }
    public AppUser Author { get; set; }
    public string RecipientId { get; set; }
    public AppUser Recipient { get; set; }
    public string ChannelId { get; set; }
    public Channel Channel { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? EditedAt { get; set; }
    public ICollection<string> AttachmentURLs { get; set; }
    public ICollection<Reaction> Reactions { get; set; } = [];
}