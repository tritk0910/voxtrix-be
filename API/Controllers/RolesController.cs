using Application.Core;
using Application.DTOs.Servers;
using Application.DTOs.Servers.Roles;
using Application.DTOs.Users;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Manages roles within a server, including retrieval of roles.
/// </summary>
[Authorize]
public class RolesController(IRoleRepository roleRepository) : BaseApiController
{
    /// <summary>
    /// Retrieves a list of roles for a given server.
    /// </summary>
    /// <param name="serverId">The ID of the server.</param>
    /// <returns>A list of roles associated with the specified server.</returns>
    [HttpGet]
    public async Task<ActionResult<Result<List<RoleDto>>>> GetRolesByServerId(string serverId)
    {
        var result = await roleRepository.GetRolesByServerIdAsync(serverId);
        if (result.Success == false) return BadRequest(result);
        return Ok(result);
    }

    [HttpGet("members")]
    public async Task<ActionResult<Result<List<UserBasicDto>>>> GetRoleMembers(string serverId, string roleId)
    {
        var result = await roleRepository.GetRoleMembersAsync(serverId, roleId);
        if (result.Success == false) return BadRequest(result);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Result<RoleDetailsDto>>> CreateRole(CreateRoleDto createRoleDto)
    {
        var result = await roleRepository.CreateRoleAsync(createRoleDto);
        if (result.Success == false) return BadRequest(result);
        return Ok(result);
    }

    [HttpPut]
    public async Task<ActionResult<Result<RoleDetailsDto>>> UpdateRole(UpdateRoleDto updateRoleDto)
    {
        var result = await roleRepository.UpdateRoleAsync(updateRoleDto);
        if (result.Success == false) return BadRequest(result);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<ActionResult<Result<bool>>> DeleteRole(string roleId)
    {
        var result = await roleRepository.DeleteRoleAsync(roleId);
        if (result.Success == false) return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("member")]
    public async Task<ActionResult<Result<ServerMemberDto>>> UpdateUserRole(UpdateOrDeleteServerMemberRoleDto updateServerMemberRoleDto)
    {
        var result = await roleRepository.UpdateUserRoleAsync(updateServerMemberRoleDto);
        if (result.Success == false) return BadRequest(result);
        return Ok(result);
    }

    [HttpDelete("member")]
    public async Task<ActionResult<Result<ServerMemberDto>>> DeleteUserRole(UpdateOrDeleteServerMemberRoleDto updateServerMemberRoleDto)
    {
        var result = await roleRepository.DeleteUserRoleAsync(updateServerMemberRoleDto);
        if (result.Success == false) return BadRequest(result);
        return Ok(result);
    }
}
