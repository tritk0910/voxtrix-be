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

    public async Task<Result<UserDetailsDto>> EditUserAsync(UserEditDto userEditDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userEditDto.Id);
        if (user == null) return Result<UserDetailsDto>.FailureResult("User not found");

        mapper.Map(userEditDto, user);
        var result = await context.SaveChangesAsync() > 0;

        if (!result) return Result<UserDetailsDto>.FailureResult("Failed to update user");

        var userDetailsDto = mapper.Map<UserDetailsDto>(user);
        return Result<UserDetailsDto>.SuccessResult(userDetailsDto, "User updated successfully");
    }

    public async Task<Result<bool>> DeleteUserAsync(string userId)
    {
        var user = await context.Users
            .Include(sv => sv.OwnedServers)
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null) return Result<bool>.FailureResult("User not found");
        if (user.OwnedServers.Count != 0) return Result<bool>.FailureResult("User owns servers, please transfer ownership or delete the servers before deleting");

        context.Users.Remove(user);
        var result = await context.SaveChangesAsync() > 0;

        if (!result) return Result<bool>.FailureResult("Failed to delete user");

        var userDetailsDto = mapper.Map<UserDetailsDto>(user);
        return Result<bool>.SuccessResult(true, "User deleted successfully");
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

    public async Task<Result<FriendResponseDto>> SendFriendRequestAsync(string userId, string targetUsername)
    {
        var user = await context.Users
            .Include(f => f.Friends)
            .Include(b => b.BlockedUsers)
            .FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null) return Result<FriendResponseDto>.FailureResult("User not found");

        var targetUser = await context.Users
            .Include(b => b.BlockedUsers)
            .FirstOrDefaultAsync(x => x.UserName == targetUsername);
        if (targetUser == null) return Result<FriendResponseDto>.FailureResult("Target user not found");

        if (userId == targetUser.Id) return Result<FriendResponseDto>.FailureResult("You cannot send friend request to yourself");

        var userBlock = user.BlockedUsers.Any(b => b.BlockedUserId == targetUser.Id);
        if (userBlock) return Result<FriendResponseDto>.FailureResult("You have blocked this user");

        var userIsBlocked = targetUser.BlockedUsers.Any(b => b.BlockedUserId == userId);
        if (userIsBlocked) return Result<FriendResponseDto>.FailureResult("You can't send friend request to this user");

        var existingRequest = await context.Friends.FirstOrDefaultAsync(f =>
            (f.UserId == userId && f.TargetId == targetUser.Id) ||
            (f.UserId == targetUser.Id && f.TargetId == userId));

        if (existingRequest != null)
        {
            if (existingRequest.UserId == targetUser.Id && existingRequest.TargetId == userId &&
                (existingRequest.Status == Status.Pending || existingRequest.Status == Status.Ignored))
            {
                if (userBlock) return Result<FriendResponseDto>.FailureResult("Cannot accept friend request, you have blocked this user");
                if (userIsBlocked) return Result<FriendResponseDto>.FailureResult("Cannot send friend request");
                existingRequest.Status = Status.Accepted;

                var result = await context.SaveChangesAsync() > 0;
                if (!result) return Result<FriendResponseDto>.FailureResult("Failed to accept friend request");

                var friendRequestDto = mapper.Map<FriendResponseDto>(existingRequest);

                return Result<FriendResponseDto>.SuccessResult(friendRequestDto, "Friend request accepted");
            }
            if (existingRequest.Status == Status.Accepted) return Result<FriendResponseDto>.FailureResult("You are already friends");
            return Result<FriendResponseDto>.FailureResult("Friend request already sent");
        }

        var friendRequest = new Friend
        {
            UserId = userId,
            TargetId = targetUser.Id,
            Status = Status.Pending
        };

        context.Friends.Add(friendRequest);
        var saveResult = await context.SaveChangesAsync() > 0;
        if (!saveResult) return Result<FriendResponseDto>.FailureResult("Failed to send friend request");

        var newFriendRequestDto = mapper.Map<FriendResponseDto>(friendRequest);

        return Result<FriendResponseDto>.SuccessResult(newFriendRequestDto, "Friend request sent");
    }

    public async Task<Result<FriendResponseDto>> IgnoreFriendRequestAsync(string requestId)
    {
        var friendRequest = await context.Friends
            .FirstOrDefaultAsync(f => f.FriendId == requestId && f.Status == Status.Pending);

        if (friendRequest == null) return Result<FriendResponseDto>.FailureResult("Friend request not found");

        friendRequest.Status = Status.Ignored;
        var saveResult = await context.SaveChangesAsync() > 0;
        if (!saveResult) return Result<FriendResponseDto>.FailureResult("Failed to ignore friend request");

        var friendRequestDto = mapper.Map<FriendResponseDto>(friendRequest);

        return Result<FriendResponseDto>.SuccessResult(friendRequestDto, "Friend request ignored");
    }

    public async Task<Result<FriendResponseDto>> AcceptFriendRequestAsync(string requestId)
    {
        var friendRequest = await context.Friends
            .FirstOrDefaultAsync(f => f.FriendId == requestId && f.Status == Status.Pending);

        if (friendRequest == null) return Result<FriendResponseDto>.FailureResult("Friend request not found");

        var user = await context.Users.Include(b => b.BlockedUsers).FirstOrDefaultAsync(x => x.Id == friendRequest.UserId);
        var targetUser = await context.Users.Include(b => b.BlockedUsers).FirstOrDefaultAsync(x => x.Id == friendRequest.TargetId);

        if (user == null || targetUser == null) return Result<FriendResponseDto>.FailureResult("User not found");

        bool isBlocked = user.BlockedUsers.Any(b => b.BlockedUserId == targetUser.Id) ||
                targetUser.BlockedUsers.Any(b => b.BlockedUserId == user.Id);

        if (isBlocked) return Result<FriendResponseDto>.FailureResult("Cannot accept friend request");

        friendRequest.Status = Status.Accepted;
        var saveResult = await context.SaveChangesAsync() > 0;
        if (!saveResult) return Result<FriendResponseDto>.FailureResult("Failed to accept friend request");

        var friendRequestDto = mapper.Map<FriendResponseDto>(friendRequest);

        return Result<FriendResponseDto>.SuccessResult(friendRequestDto, "Friend request accepted");
    }

    public async Task<Result<bool>> RemoveFriendRequestAsync(string requestId)
    {
        var friendRequest = await context.Friends
            .FirstOrDefaultAsync(f => f.FriendId == requestId);

        if (friendRequest == null) return Result<bool>.FailureResult("Friend request not found");

        context.Friends.Remove(friendRequest);
        var result = await context.SaveChangesAsync() > 0;
        if (!result) return Result<bool>.FailureResult("Failed to remove friend request");

        return Result<bool>.SuccessResult(true, "Friend request removed successfully");
    }

    public async Task<Result<bool>> RemoveFriendAsync(string userId, string targetId)
    {
        var friendship = await context.Friends
            .FirstOrDefaultAsync(f => f.UserId == userId && f.TargetId == targetId || f.UserId == targetId && f.TargetId == userId && f.Status == Status.Accepted);

        if (friendship == null) return Result<bool>.FailureResult("You are not friends with this user");

        context.Friends.Remove(friendship);
        var result = await context.SaveChangesAsync() > 0;
        if (!result) return Result<bool>.FailureResult("Failed to remove friend");

        return Result<bool>.SuccessResult(true, "Friend removed successfully");
    }

    public async Task<IQueryable<FriendDto>> GetPendingFriendRequestsAsync(string userId, DefaultParams defaultParams)
    {
        var query = context.Friends
            .Where(f => f.UserId == userId && f.Status == Status.Pending || f.Status == Status.Ignored)
            .Select(f => new FriendDto
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

    public async Task<IQueryable<FriendDto>> GetIncomingFriendRequestsAsync(string userId, DefaultParams defaultParams)
    {
        var query = context.Friends
            .Where(f => f.TargetId == userId && f.Status == Status.Pending)
            .Select(f => new FriendDto
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

    public async Task<Result<BlockedUserDto>> BlockUserAsync(string userId, string targetId)
    {
        if (userId == targetId) return Result<BlockedUserDto>.FailureResult("Users cannot block themselves");

        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null) return Result<BlockedUserDto>.FailureResult("User not found");

        var targetUser = await context.Users.FirstOrDefaultAsync(x => x.Id == targetId);
        if (targetUser == null) return Result<BlockedUserDto>.FailureResult("Target user not found");

        if (await context.UserBlocks.AnyAsync(ub => ub.UserId == userId && ub.BlockedUserId == targetId))
            return Result<BlockedUserDto>.FailureResult("User already blocked");

        // Remove friend if exists when blocking user
        context.Friends.RemoveRange(context.Friends.Where(f =>
            (f.UserId == userId && f.TargetId == targetId) ||
            (f.UserId == targetId && f.TargetId == userId)));

        var userBlock = new UserBlock
        {
            UserId = userId,
            BlockedUserId = targetId
        };

        context.UserBlocks.Add(userBlock);
        var saveResult = await context.SaveChangesAsync() > 0;

        if (!saveResult) return Result<BlockedUserDto>.FailureResult("Failed to block user");

        var blockedUserDto = new BlockedUserDto
        {
            BlockId = userBlock.UserBlockId,
            UserId = targetUser.Id,
            DisplayName = targetUser.DisplayName,
            Username = targetUser.UserName,
            Avatar = targetUser.Avatar
        };

        return Result<BlockedUserDto>.SuccessResult(blockedUserDto, "User blocked successfully");
    }

    public async Task<Result<bool>> UnblockUserAsync(string blockId)
    {
        var userBlock = await context.UserBlocks
            .FirstOrDefaultAsync(ub => ub.UserBlockId == blockId);

        if (userBlock == null) return Result<bool>.FailureResult("User block not found");

        context.UserBlocks.Remove(userBlock);
        var saveResult = await context.SaveChangesAsync() > 0;

        if (!saveResult) return Result<bool>.FailureResult("Failed to unblock user");

        return Result<bool>.SuccessResult(true, "User unblocked successfully");
    }
}