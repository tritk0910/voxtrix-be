using Domain.Entities;

namespace Application.DTOs.Channels;

public class ChannelDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ServerId { get; set; }
    public string ParentChannelId { get; set; }
    public ChannelType Type { get; set; }
}