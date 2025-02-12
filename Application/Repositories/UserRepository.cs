using Application.Core;
using Application.DTOs.Accounts;
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
    public void AddUser(AppUser user)
    {
        context.Users.Add(user);
    }

    public IQueryable<UserDto> GetAllUsersAsync(DefaultParams defaultParams)
    {
        var query = context.Users.ProjectTo<UserDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(defaultParams.Search))
        {
            query = query.Where(x => x.Username.Contains(defaultParams.Search));
        }

        return query;
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

    public async Task<UserDetailDto> GetUserByIdAsync(string id)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
        var result = mapper.Map<UserDetailDto>(user);
        return result;
    }

    public async Task<string> EditUserAsync(UserEditDto UserEditDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == UserEditDto.Username);

        if (user == null) return "User not found";

        mapper.Map(UserEditDto, user);

        await context.SaveChangesAsync();
        return "User updated successfully";
    }
}