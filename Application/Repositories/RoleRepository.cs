using Application.Core;
using Application.DTOs.Servers;
using Application.DTOs.Servers.Roles;
using Application.DTOs.Users;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repositories;

public class RoleRepository(DataContext context, IMapper mapper) : IRoleRepository
{
    public async Task<Result<List<RoleDto>>> GetRolesByServerIdAsync(string serverId)
    {
        var roles = await context.ServerRoles
            .Where(r => r.ServerId == serverId)
            .AsNoTracking()
            .Include(r => r.ServerMemberRoles)
                .ThenInclude(r => r.ServerMember)
                .ThenInclude(r => r.Member)
            .ProjectTo<RoleDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return Result<List<RoleDto>>.SuccessResult(roles);
    }

    public async Task<Result<List<UserBasicDto>>> GetRoleMembersAsync(string serverId, string roleId)
    {
        IQueryable<ServerMemberRole> query;

        if (string.IsNullOrEmpty(roleId))
        {
            var defaultRole = await context.ServerRoles
                .Where(r => r.ServerId == serverId && r.IsDefault)
                .FirstOrDefaultAsync();

            if (defaultRole == null)
            {
                return Result<List<UserBasicDto>>.FailureResult("Default role not found.");
            }

            query = context.ServerMemberRoles
                .Where(smr => smr.RoleId == defaultRole.RoleId && smr.ServerMember.ServerId == serverId);
        }
        else
        {
            query = context.ServerMemberRoles
                .Where(smr => smr.RoleId == roleId && smr.ServerMember.ServerId == serverId);
        }

        if (query == null)
        {
            return Result<List<UserBasicDto>>.FailureResult("Server not found.");
        }

        var members = await query
            .AsNoTracking()
            .Include(smr => smr.ServerMember)
                .ThenInclude(sm => sm.Member)
            .Select(smr => smr.ServerMember)
            .ProjectTo<UserBasicDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return Result<List<UserBasicDto>>.SuccessResult(members);
    }

    public async Task<Result<RoleDetailsDto>> CreateRoleAsync(CreateRoleDto createRoleDto)
    {
        var role = mapper.Map<Role>(createRoleDto);

        context.Add(role);
        var result = await context.SaveChangesAsync() > 0;

        return result
            ? Result<RoleDetailsDto>.SuccessResult(mapper.Map<RoleDetailsDto>(role), "Successfully created role.")
            : Result<RoleDetailsDto>.FailureResult("Failed to create role.");
    }

    public async Task<Result<RoleDetailsDto>> UpdateRoleAsync(UpdateRoleDto updateRoleDto)
    {
        var role = await context.ServerRoles
            .Where(r => r.RoleId == updateRoleDto.RoleId)
            .FirstOrDefaultAsync();

        if (role == null)
        {
            return Result<RoleDetailsDto>.FailureResult("Role not found.");
        }

        mapper.Map(updateRoleDto, role);
        var result = await context.SaveChangesAsync() > 0;

        return result
            ? Result<RoleDetailsDto>.SuccessResult(mapper.Map<RoleDetailsDto>(role), "Successfully updated role.")
            : Result<RoleDetailsDto>.FailureResult("Failed to update role.");
    }

    public async Task<Result<bool>> DeleteRoleAsync(string roleId)
    {
        var role = await context.ServerRoles
            .Where(r => r.RoleId == roleId)
            .FirstOrDefaultAsync();

        if (role == null)
        {
            return Result<bool>.FailureResult("Role not found.");
        }

        context.Remove(role);
        var result = await context.SaveChangesAsync() > 0;

        return result
            ? Result<bool>.SuccessResult(true, "Successfully deleted role.")
            : Result<bool>.FailureResult("Failed to delete role.");
    }

    public async Task<Result<ServerMemberDto>> UpdateUserRoleAsync(UpdateOrDeleteServerMemberRoleDto updateServerMemberRoleDto)
    {
        var serverMemberRole = await context.ServerMemberRoles
            .Where(smr => smr.ServerMember.MemberId == updateServerMemberRoleDto.UserId && smr.ServerMember.ServerId == updateServerMemberRoleDto.ServerId)
            .Include(smr => smr.ServerMember)
                .ThenInclude(smr => smr.Member)
            .Include(smr => smr.Role)
            .FirstOrDefaultAsync();

        if (serverMemberRole == null)
        {
            return Result<ServerMemberDto>.FailureResult("User not found in server.");
        }

        var newUserRole = new ServerMemberRole
        {
            RoleId = updateServerMemberRoleDto.RoleId,
            ServerMember = serverMemberRole.ServerMember
        };

        context.Add(newUserRole);
        var result = await context.SaveChangesAsync() > 0;

        var serverMember = await context.ServerMembers
            .Where(sm => sm.MemberId == updateServerMemberRoleDto.UserId && sm.ServerId == updateServerMemberRoleDto.ServerId)
            .AsNoTracking()
            .Include(sm => sm.Member)
            .Include(sm => sm.ServerMemberRoles)
                .ThenInclude(smr => smr.Role)
            .ProjectTo<ServerMemberDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return result
            ? Result<ServerMemberDto>.SuccessResult(serverMember, "Successfully updated user role.")
            : Result<ServerMemberDto>.FailureResult("Failed to update user role.");
    }

    public async Task<Result<ServerMemberDto>> DeleteUserRoleAsync(UpdateOrDeleteServerMemberRoleDto updateServerMemberRoleDto)
    {
        var serverMemberRole = await context.ServerMemberRoles
            .Where(smr => smr.ServerMember.MemberId == updateServerMemberRoleDto.UserId && smr.ServerMember.ServerId == updateServerMemberRoleDto.ServerId && smr.RoleId == updateServerMemberRoleDto.RoleId)
            .Include(smr => smr.ServerMember)
                .ThenInclude(smr => smr.Member)
            .Include(smr => smr.Role)
            .FirstOrDefaultAsync();

        if (serverMemberRole == null)
        {
            return Result<ServerMemberDto>.FailureResult("User not found in server.");
        }

        context.Remove(serverMemberRole);
        var result = await context.SaveChangesAsync() > 0;

        var serverMember = await context.ServerMembers
            .Where(sm => sm.MemberId == updateServerMemberRoleDto.UserId && sm.ServerId == updateServerMemberRoleDto.ServerId)
            .AsNoTracking()
            .Include(sm => sm.Member)
            .Include(sm => sm.ServerMemberRoles)
                .ThenInclude(smr => smr.Role)
            .ProjectTo<ServerMemberDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return result
            ? Result<ServerMemberDto>.SuccessResult(serverMember, "Successfully removed user role.")
            : Result<ServerMemberDto>.FailureResult("Failed to remove user role.");
    }
}