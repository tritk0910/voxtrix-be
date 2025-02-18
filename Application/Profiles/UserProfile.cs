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
        CreateMap<UserBasicDto, AppUser>()
            .ReverseMap();
        CreateMap<UserDetailsDto, AppUser>()
            .ReverseMap();
        CreateMap<UserEditDto, AppUser>()
            .ReverseMap();
        CreateMap<Friend, FriendResponseDto>()
            .ForMember(dest => dest.RequestId, opt => opt.MapFrom(src => src.FriendId))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ReverseMap();
        CreateMap<Friend, UserBasicDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TargetId))
            .ReverseMap();
        CreateMap<UserBlock, BlockedUserDto>()
            .ForMember(dest => dest.BlockId, opt => opt.MapFrom(src => src.UserBlockId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.BlockedUserId))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.BlockedUser.UserName))
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.BlockedUser.DisplayName))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.BlockedUser.Avatar))
            .ReverseMap();
    }
}