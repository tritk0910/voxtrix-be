using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Channel
{
    [Key]
    public string ChannelId { get; set; }
    public string Description { get; set; }
    public string ChannelName { get; set; }
    public string ServerId { get; set; }
    public ChannelType ChannelType { get; set; }
    public string ParentChannelId { get; set; }
    public Server Server { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Channel ParentChannel { get; set; }
    public ICollection<Message> Messages { get; set; } = [];
}

public enum ChannelType
{
    Text,
    Voice,
    Category
}