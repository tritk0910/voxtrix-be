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

    /// <summary>
    /// Retrieves a list of members for a given role in a server.
    /// </summary>
    /// <param name="serverId">The ID of the server.</param>
    /// <param name="roleId">The ID of the role.</param>
    /// <returns>A list of members associated with the specified role.</returns>
    [HttpGet("members")]
    public async Task<ActionResult<Result<List<UserBasicDto>>>> GetRoleMembers(string serverId, string roleId)
    {
        var result = await roleRepository.GetRoleMembersAsync(serverId, roleId);
        if (result.Success == false) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new role in a server.
    /// </summary>
    /// <param name="createRoleDto">The details of the role to create.</param>
    /// <returns>The created role.</returns>
    [HttpPost]
    public async Task<ActionResult<Result<RoleDetailsDto>>> CreateRole(CreateRoleDto createRoleDto)
    {
        var result = await roleRepository.CreateRoleAsync(createRoleDto);
        if (result.Success == false) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing role in a server.
    /// </summary>
    /// <param name="updateRoleDto">The details of the role to update.</param>
    /// <returns>The updated role.</returns>
    [HttpPut]
    public async Task<ActionResult<Result<RoleDetailsDto>>> UpdateRole(UpdateRoleDto updateRoleDto)
    {
        var result = await roleRepository.UpdateRoleAsync(updateRoleDto);
        if (result.Success == false) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a role from a server.
    /// </summary>
    /// <param name="roleId">The ID of the role to delete.</param>
    /// <returns>A boolean indicating whether the deletion was successful.</returns>
    [HttpDelete]
    public async Task<ActionResult<Result<bool>>> DeleteRole(string roleId)
    {
        var result = await roleRepository.DeleteRoleAsync(roleId);
        if (result.Success == false) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Updates a user's role in a server.
    /// </summary>
    /// <param name="updateServerMemberRoleDto">The details of the user role to update.</param>
    /// <returns>The updated server member.</returns>
    [HttpPut("member")]
    public async Task<ActionResult<Result<ServerMemberDto>>> UpdateUserRole(UpdateOrDeleteServerMemberRoleDto updateServerMemberRoleDto)
    {
        var result = await roleRepository.UpdateUserRoleAsync(updateServerMemberRoleDto);
        if (result.Success == false) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a user's role in a server.
    /// </summary>
    /// <param name="updateServerMemberRoleDto">The details of the user role to delete.</param>
    /// <returns>The updated server member.</returns>
    [HttpDelete("member")]
    public async Task<ActionResult<Result<ServerMemberDto>>> DeleteUserRole(UpdateOrDeleteServerMemberRoleDto updateServerMemberRoleDto)
    {
        var result = await roleRepository.DeleteUserRoleAsync(updateServerMemberRoleDto);
        if (result.Success == false) return BadRequest(result);
        return Ok(result);
    }
}
