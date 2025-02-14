using Application.Core;
using Application.DTOs.Friends;
using Application.DTOs.Users;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<IQueryable<UserDto>> GetAllUsersAsync(DefaultParams defaultParams);
    Task<UserDetailsDto> GetUserByIdAsync(string id);
    Task<string> EditUserAsync(UserEditDto userEditDto);
    Task<string> DeleteUserAsync(string userId);
    Task<IQueryable<UserBasicDto>> GetFriendsAsync(string userId, DefaultParams defaultParams);
    Task<string> SendFriendRequestAsync(string userId, string friendId);
    Task<string> IgnoreFriendRequestAsync(string requestId);
    Task<string> RemoveFriendRequestAsync(string requestId);
    Task<string> AcceptFriendRequestAsync(string requestId);
    Task<string> RemoveFriendAsync(string userId, string targetId);
    Task<IQueryable<FriendRequestDto>> GetPendingFriendRequestsAsync(string userId, DefaultParams defaultParams);
    Task<IQueryable<FriendRequestDto>> GetIncomingFriendRequestsAsync(string userId, DefaultParams defaultParams);
}
