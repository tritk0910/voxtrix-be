using Application.DTOs.Invites;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class InviteProfile : Profile
{
    public InviteProfile()
    {
        CreateMap<Invite, InviteDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.InviteId))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.InviteCode))
            .ReverseMap();
        CreateMap<Invite, CreateInviteDto>()
            .ReverseMap();
        CreateMap<Invite, UpdateInviteDto>()
            .ReverseMap();
    }
}