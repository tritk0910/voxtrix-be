using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class VoiceState
{
    [Key]
    public string VoiceStateId { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public string ChannelId { get; set; }
    public Channel Channel { get; set; }
    public bool IsDeafened { get; set; }
    public bool IsMuted { get; set; }
    public bool IsSelfDeafened { get; set; }
    public bool IsSelfMuted { get; set; }
    public bool IsSuppressed { get; set; }
    public bool IsStreaming { get; set; }
    public bool IsVideoEnabled { get; set; }
    public bool IsVoiceActivityEnabled { get; set; }
    public bool IsVoiceActivityDetected { get; set; }
    public bool IsVoiceConnected { get; set; }
    public bool IsVoiceDisconnected { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public DateTime LeftAt { get; set; }
}