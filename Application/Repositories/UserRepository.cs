using Application.Core;
using Application.DTOs.Accounts;
using Application.DTOs.Friends;
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
            .Where(f => f.UserId == userId || f.FriendId == userId && f.Status == Status.Accepted)
            .Select(f => f.UserId == userId ? f.Friend : f.User)
            .ProjectTo<UserBasicDto>(mapper.ConfigurationProvider)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(defaultParams.Search))
        {
            friendsQuery = friendsQuery.Where(x => x.Username.Contains(defaultParams.Search));
        }

        return await Task.FromResult(friendsQuery);
    }

    public async Task<string> SendFriendRequestAsync(string userId, string friendId)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null) return "User not found";

        var targetUser = await context.Users.FirstOrDefaultAsync(x => x.Id == friendId);
        if (targetUser == null) return "Target user not found";

        var friendRequest = new FriendshipRelation
        {
            UserId = userId,
            FriendId = friendId,
            Status = Status.Pending
        };

        context.Friends.Add(friendRequest);
        var result = await context.SaveChangesAsync() > 0;
        if (result) return "Friend request sent";
        return "Failed to send friend request";
    }

    public async Task<string> IgnoreFriendRequestAsync(string requestId)
    {
        var friendRequest = await context.Friends
            .FirstOrDefaultAsync(f => f.FriendshipRelationId == requestId && f.Status == Status.Pending);

        if (friendRequest == null) return "Friend request not found";

        friendRequest.Status = Status.Ignored;
        var result = await context.SaveChangesAsync() > 0;
        if (result) return "Friend request ignored";
        return "Failed to ignore friend request";
    }

    public async Task<string> AcceptFriendRequestAsync(string requestId)
    {
        var friendRequest = await context.Friends
            .FirstOrDefaultAsync(f => f.FriendshipRelationId == requestId && f.Status == Status.Pending);

        if (friendRequest == null) return "Friend request not found";

        friendRequest.Status = Status.Accepted;
        var result = await context.SaveChangesAsync() > 0;
        if (result) return "Friend request accepted";
        return "Failed to accept friend request";
    }

    public async Task<string> RemoveFriendRequestAsync(string requestId)
    {
        var friendRequest = await context.Friends
            .FirstOrDefaultAsync(f => f.FriendshipRelationId == requestId);

        if (friendRequest == null) return "Friend request not found";

        context.Friends.Remove(friendRequest);
        var result = await context.SaveChangesAsync() > 0;
        if (result) return "Friend request removed";
        return "Failed to remove friend request";
    }

    public async Task<string> RemoveFriendAsync(string userId, string targetId)
    {
        var friendship = await context.Friends
            .FirstOrDefaultAsync(f => f.UserId == userId && f.FriendId == targetId || f.UserId == targetId && f.FriendId == userId && f.Status == Status.Accepted);

        if (friendship == null) return "Friendship not found";

        context.Friends.Remove(friendship);
        var result = await context.SaveChangesAsync() > 0;
        if (result) return "Friend removed successfully";
        return "Failed to remove friend";
    }

    public async Task<IQueryable<FriendRequestDto>> GetPendingFriendRequestsAsync(string userId, DefaultParams defaultParams)
    {
        var query = context.Friends
            .Where(f => f.UserId == userId && f.Status == Status.Pending || f.Status == Status.Ignored)
            .Select(f => new FriendRequestDto
            {
                FriendRequestId = f.FriendshipRelationId,
                UserId = f.FriendId,
                UserName = f.Friend.UserName,
                DisplayName = f.Friend.DisplayName,
                Avatar = f.Friend.Avatar
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
            .Where(f => f.FriendId == userId && f.Status == Status.Pending)
            .Select(f => new FriendRequestDto
            {
                FriendRequestId = f.FriendshipRelationId,
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
}