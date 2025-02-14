using Application.DTOs.Accounts;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<RegisterDto, AppUser>();
        CreateMap<LoginDto, AppUser>();
    }
}