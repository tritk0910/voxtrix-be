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
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.CreatedBy))
            .ReverseMap();
        CreateMap<Invite, CreateInviteDto>()
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.CreatedBy))
            .ReverseMap();
    }
}