using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<bool> UserExists(string username, string email);
    Task<AppUser> UsernameOrEmailExists(LoginDto loginDto);
    void AddUser(AppUser user);
    Task<int> SaveAllAsync();
    Task<AppUser> GetUserByUsernameAsync(string username);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
}
