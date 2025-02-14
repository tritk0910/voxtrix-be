namespace Application.DTOs.Servers;

public class ServerRoleDto
{
    public string RoleId { get; set; }
    public string RoleName { get; set; }
}

public class ServerRoleDetailsDto : ServerRoleDto
{
    public string RoleColor { get; set; }
    public bool IsMentionable { get; set; }
    public bool IsDisplayedSeparately { get; set; }
    public bool IsHoisted { get; set; }
    public bool IsManaged { get; set; }
    public bool IsDefault { get; set; }
    public int Position { get; set; }
}