using Application.Core;
using Application.DTOs.Servers;
using Application.DTOs.Servers.Roles;
using Application.DTOs.Users;

namespace Application.Interfaces;

public interface IRoleRepository
{
    Task<Result<List<RoleDto>>> GetRolesByServerIdAsync(string serverId);
    Task<Result<List<UserBasicDto>>> GetRoleMembersAsync(string serverId, string roleId);
    Task<Result<RoleDetailsDto>> CreateRoleAsync(CreateRoleDto createRoleDto);
    Task<Result<RoleDetailsDto>> UpdateRoleAsync(UpdateRoleDto updateRoleDto);
    Task<Result<bool>> DeleteRoleAsync(string roleId);
    Task<Result<ServerMemberDto>> UpdateUserRoleAsync(UpdateOrDeleteServerMemberRoleDto updateServerMemberRoleDto);
    Task<Result<ServerMemberDto>> DeleteUserRoleAsync(UpdateOrDeleteServerMemberRoleDto updateServerMemberRoleDto);
}