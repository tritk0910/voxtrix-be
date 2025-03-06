using Application.DTOs.Invites;
using Application.DTOs.Servers.Roles;

namespace Application.DTOs.Servers;

public class ServerDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Avatar { get; set; }
}


public class ServerTransferDto : ServerDto
{
    public string OwnerId { get; set; }
}

public class ServerBasicDto : ServerTransferDto
{
    public string BannerImage { get; set; }
}

public class ServerDetailsDto : ServerBasicDto
{
    public List<InviteDto> Invites { get; set; }
    public List<BanDto> Bans { get; set; }
    public List<RoleDto> Roles { get; set; }
}