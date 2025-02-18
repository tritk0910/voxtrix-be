using Application.Core;
using Application.DTOs.Users;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<IQueryable<UserDto>> GetAllUsersAsync(DefaultParams defaultParams);
    Task<UserDetailsDto> GetUserByIdAsync(string id);
    Task<Result<UserDetailsDto>> EditUserAsync(UserEditDto userEditDto);
    Task<Result<bool>> DeleteUserAsync(string userId);
    Task<IQueryable<UserBasicDto>> GetFriendsAsync(string userId, DefaultParams defaultParams);
    Task<Result<FriendResponseDto>> SendFriendRequestAsync(string userId, string targetUsername);
    Task<Result<FriendResponseDto>> IgnoreFriendRequestAsync(string requestId);
    Task<Result<bool>> RemoveFriendRequestAsync(string requestId);
    Task<Result<FriendResponseDto>> AcceptFriendRequestAsync(string requestId);
    Task<Result<bool>> RemoveFriendAsync(string userId, string targetId);
    Task<IQueryable<FriendDto>> GetPendingFriendRequestsAsync(string userId, DefaultParams defaultParams);
    Task<IQueryable<FriendDto>> GetIncomingFriendRequestsAsync(string userId, DefaultParams defaultParams);
    Task<IQueryable<BlockedUserDto>> GetBlockedUsersAsync(string userId, DefaultParams defaultParams);
    Task<Result<BlockedUserDto>> BlockUserAsync(string userId, string targetId);
    Task<Result<bool>> UnblockUserAsync(string blockId);
}
