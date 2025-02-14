using Application.DTOs.Users;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserDto, AppUser>()
            .ReverseMap();
        CreateMap<UserDetailsDto, AppUser>()
            .ReverseMap();
        CreateMap<UserEditDto, AppUser>()
            .ReverseMap();
    }
}