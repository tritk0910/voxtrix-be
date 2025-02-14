using Application.Core;
using Application.DTOs.Users;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repositories;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    public async Task<IQueryable<UserDto>> GetAllUsersAsync(DefaultParams defaultParams)
    {
        var query = context.Users.ProjectTo<UserDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(defaultParams.Search))
        {
            query = query.Where(x => x.Username.Contains(defaultParams.Search));
        }

        return await Task.FromResult(query);
    }

    public async Task<UserDetailsDto> GetUserByIdAsync(string id)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
        var result = mapper.Map<UserDetailsDto>(user);
        return result;
    }

    public async Task<string> EditUserAsync(UserEditDto UserEditDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == UserEditDto.Id);

        if (user == null) return "User not found";

        mapper.Map(UserEditDto, user);

        await context.SaveChangesAsync();
        return "User updated successfully";
    }

    public async Task<string> DeleteUserAsync(string userId)
    {
        var user = await context.Users
            .Include(sv => sv.OwnedServers)
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null) return "User not found";
        if (user.OwnedServers.Count != 0) return "User owns servers, please transfer ownership or delete the servers before deleting";

        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return "User deleted successfully";
    }


    public async Task<IQueryable<UserBasicDto>> GetFriendsAsync(string userId, DefaultParams defaultParams)
    {
        var friendsQuery = context.Friends
            .Where(f => (f.UserId == userId || f.TargetId == userId) && f.Status == Status.Accepted)
            .Select(f => f.UserId == userId ? f.Target : f.User)
            .ProjectTo<UserBasicDto>(mapper.ConfigurationProvider)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(defaultParams.Search))
        {
            friendsQuery = friendsQuery.Where(x => x.Username.Contains(defaultParams.Search));
        }

        return await Task.FromResult(friendsQuery);
    }

    public async Task<string> SendFriendRequestAsync(string userId, string targetUsername)
    {
        var user = await context.Users
            .Include(f => f.Friends)
            .Include(b => b.BlockedUsers)
            .FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null) return "User not found";

        var targetUser = await context.Users
            .Include(b => b.BlockedUsers)
            .FirstOrDefaultAsync(x => x.UserName == targetUsername);
        if (targetUser == null) return "Target user not found";

        if (userId == targetUser.Id) return "You cannot send friend request to yourself";

        var userBlock = user.BlockedUsers.Any(b => b.BlockedUserId == targetUser.Id);
        if (userBlock) return "You have blocked this user";

        var userIsBlocked = targetUser.BlockedUsers.Any(b => b.BlockedUserId == userId);
        if (userIsBlocked) return "You can't send friend request to this user";

        var existingRequest = await context.Friends.FirstOrDefaultAsync(f =>
            (f.UserId == userId && f.TargetId == targetUser.Id) ||
            (f.UserId == targetUser.Id && f.TargetId == userId));

        if (existingRequest != null)
        {
            if (existingRequest.UserId == targetUser.Id && existingRequest.TargetId == userId &&
                (existingRequest.Status == Status.Pending || existingRequest.Status == Status.Ignored))
            {
                if (userBlock) return "Cannot accept friend request, you have blocked this user";
                if (userIsBlocked) return "Cannot send friend request";
                existingRequest.Status = Status.Accepted;

                var result = await context.SaveChangesAsync() > 0;
                return result ? "Friend request accepted" : "Failed to accept friend request";
            }
            if (existingRequest.Status == Status.Accepted) return "You are already friends";
            return "Friend request already sent";
        }

        var friendRequest = new Friend
        {
            UserId = userId,
            TargetId = targetUser.Id,
            Status = Status.Pending
        };

        context.Friends.Add(friendRequest);
        var saveResult = await context.SaveChangesAsync() > 0;
        return saveResult ? "Friend request sent" : "Failed to send friend request";
    }

    public async Task<string> IgnoreFriendRequestAsync(string requestId)
    {
        var friendRequest = await context.Friends
            .FirstOrDefaultAsync(f => f.FriendId == requestId && f.Status == Status.Pending);

        if (friendRequest == null) return "Friend request not found";

        friendRequest.Status = Status.Ignored;
        var result = await context.SaveChangesAsync() > 0;
        return result ? "Friend request ignored" : "Failed to ignore friend request";
    }

    public async Task<string> AcceptFriendRequestAsync(string requestId)
    {
        var friendRequest = await context.Friends
            .FirstOrDefaultAsync(f => f.FriendId == requestId && f.Status == Status.Pending);

        if (friendRequest == null) return "Friend request not found";

        var user = await context.Users.Include(b => b.BlockedUsers).FirstOrDefaultAsync(x => x.Id == friendRequest.UserId);
        var targetUser = await context.Users.Include(b => b.BlockedUsers).FirstOrDefaultAsync(x => x.Id == friendRequest.TargetId);

        if (user == null || targetUser == null) return "User not found";

        bool isBlocked = user.BlockedUsers.Any(b => b.BlockedUserId == targetUser.Id) ||
                targetUser.BlockedUsers.Any(b => b.BlockedUserId == user.Id);

        if (isBlocked) return "Cannot accept friend request";

        friendRequest.Status = Status.Accepted;
        var result = await context.SaveChangesAsync() > 0;
        return result ? "Friend request accepted" : "Failed to accept friend request";
    }

    public async Task<string> RemoveFriendRequestAsync(string requestId)
    {
        var friendRequest = await context.Friends
            .FirstOrDefaultAsync(f => f.FriendId == requestId);

        if (friendRequest == null) return "Friend request not found";

        context.Friends.Remove(friendRequest);
        var result = await context.SaveChangesAsync() > 0;
        return result ? "Friend request removed" : "Failed to remove friend request";
    }

    public async Task<string> RemoveFriendAsync(string userId, string targetId)
    {
        var friendship = await context.Friends
            .FirstOrDefaultAsync(f => f.UserId == userId && f.TargetId == targetId || f.UserId == targetId && f.TargetId == userId && f.Status == Status.Accepted);

        if (friendship == null) return "You are not friends with this user";

        context.Friends.Remove(friendship);
        var result = await context.SaveChangesAsync() > 0;
        return result ? "Friend removed successfully" : "Failed to remove friend";
    }

    public async Task<IQueryable<FriendRequestDto>> GetPendingFriendRequestsAsync(string userId, DefaultParams defaultParams)
    {
        var query = context.Friends
            .Where(f => f.UserId == userId && f.Status == Status.Pending || f.Status == Status.Ignored)
            .Select(f => new FriendRequestDto
            {
                RequestId = f.FriendId,
                UserId = f.TargetId,
                UserName = f.Target.UserName,
                DisplayName = f.Target.DisplayName,
                Avatar = f.Target.Avatar
            })
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(defaultParams.Search))
        {
            query = query.Where(x => x.UserName.Contains(defaultParams.Search));
        }

        return await Task.FromResult(query);
    }

    public async Task<IQueryable<FriendRequestDto>> GetIncomingFriendRequestsAsync(string userId, DefaultParams defaultParams)
    {
        var query = context.Friends
            .Where(f => f.TargetId == userId && f.Status == Status.Pending)
            .Select(f => new FriendRequestDto
            {
                RequestId = f.FriendId,
                UserId = f.UserId,
                UserName = f.User.UserName,
                DisplayName = f.User.DisplayName,
                Avatar = f.User.Avatar
            })
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(defaultParams.Search))
        {
            query = query.Where(x => x.UserName.Contains(defaultParams.Search));
        }

        return await Task.FromResult(query);
    }

    public async Task<IQueryable<BlockedUserDto>> GetBlockedUsersAsync(string userId, DefaultParams defaultParams)
    {
        var query = context.UserBlocks
            .Where(ub => ub.UserId == userId)
            .Include(ub => ub.BlockedUser)
            .ProjectTo<BlockedUserDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(defaultParams.Search))
        {
            query = query.Where(x => x.Username.Contains(defaultParams.Search));
        }

        return await Task.FromResult(query);
    }

    public async Task<string> BlockUserAsync(string userId, string targetId)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null) return "User not found";

        var targetUser = await context.Users.FirstOrDefaultAsync(x => x.Id == targetId);
        if (targetUser == null) return "Target user not found";

        if (await context.UserBlocks.AnyAsync(ub => ub.UserId == userId && ub.BlockedUserId == targetId))
            return "User already blocked";

        // Remove friendship if exists when blocking user
        context.Friends.RemoveRange(context.Friends.Where(f =>
            (f.UserId == userId && f.TargetId == targetId) ||
            (f.UserId == targetId && f.TargetId == userId)));

        context.UserBlocks.Add(new UserBlock
        {
            UserId = userId,
            BlockedUserId = targetId
        });

        return await context.SaveChangesAsync() > 0 ? "User blocked" : "Failed to block user";
    }

    public async Task<string> UnblockUserAsync(string blockId)
    {
        var userBlock = await context.UserBlocks
            .FirstOrDefaultAsync(ub => ub.UserBlockId == blockId);

        if (userBlock == null) return "User block not found";

        context.UserBlocks.Remove(userBlock);
        return await context.SaveChangesAsync() > 0 ? "User unblocked" : "Failed to unblock user";
    }
}