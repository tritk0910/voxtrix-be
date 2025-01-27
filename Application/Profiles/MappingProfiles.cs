using Application.DTOs.Accounts;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<AppUser, UserDto>();
        CreateMap<UserDto, AppUser>();
        CreateMap<RegisterDto, AppUser>();
        CreateMap<LoginDto, AppUser>();
    }
}