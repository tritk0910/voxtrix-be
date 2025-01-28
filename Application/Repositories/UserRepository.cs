using Application.DTOs.Accounts;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repositories;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    public void AddUser(AppUser user)
    {
        context.Users.Add(user);
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await context.Users.ToListAsync();
        var result = mapper.Map<IEnumerable<UserDto>>(users);
        return result;
    }

    public async Task<AppUser> GetUserByUsernameAsync(string username)
    {
        return await context.Users.FirstOrDefaultAsync(x => x.UserName == username);
    }

    public async Task<int> SaveAllAsync()
    {
        return await context.SaveChangesAsync();
    }

    public async Task<bool> UserExists(string username, string email)
    {
        return await context.Users.AnyAsync(x => x.UserName == username || x.Email == email);
    }

    public async Task<AppUser> UsernameOrEmailExists(LoginDto loginDto)
    {
        return await context.Users.FirstOrDefaultAsync(x =>
            x.UserName == loginDto.UsernameOrEmail.ToLower() || x.Email == loginDto.UsernameOrEmail.ToLower());

    }
}