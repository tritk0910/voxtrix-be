namespace Application.DTOs.Message;

public class MessageDto
{
    public string MessageId { get; set; }
    public string AuthorId { get; set; }
    public string ChannelId { get; set; }
    public string Content { get; set; }
    public List<string> AttachmentURLs { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime EditedAt { get; set; }
}