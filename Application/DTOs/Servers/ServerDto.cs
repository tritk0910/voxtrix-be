using Application.DTOs.Channels;
using Application.DTOs.Invites;

namespace Application.DTOs.Servers;

public class ServerDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Avatar { get; set; }
}

public class ServerBasicDto : ServerDto
{
    public string BannerImage { get; set; }
    public string OwnerId { get; set; }
    public List<ChannelDto> Channels { get; set; }
    public List<ServerMemberDto> Members { get; set; }
}

public class ServerDetailsDto : ServerBasicDto
{
    public List<InviteDto> Invites { get; set; }
    public List<BanDto> Bans { get; set; }
    public List<ServerRoleDto> Roles { get; set; }
}