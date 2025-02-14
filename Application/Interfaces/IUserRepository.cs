
using Application.Core;
using Application.DTOs.Accounts;
using Application.DTOs.Users;
using Domain.Entities;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<bool> UserExists(string username, string email);
    Task<AppUser> UsernameOrEmailExists(LoginDto loginDto);
    void AddUser(AppUser user);
    Task<int> SaveAllAsync();
    Task<AppUser> GetUserByUsernameAsync(string username);
    IQueryable<UserDto> GetAllUsersAsync(DefaultParams defaultParams);
    Task<UserDetailsDto> GetUserByIdAsync(string id);
    Task<string> EditUserAsync(UserEditDto userEditDto);
    Task<string> DeleteUserAsync(string userId);
}
