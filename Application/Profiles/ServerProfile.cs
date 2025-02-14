using Application.DTOs.Servers;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class ServerProfile : Profile
{
    public ServerProfile()
    {
        CreateMap<Server, ServerDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ServerId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ServerName))
            .ReverseMap();
        CreateMap<CreateServerDto, Server>()
            .ForMember(dest => dest.ServerName, opt => opt.MapFrom(src => src.Name))
            .ReverseMap();
        CreateMap<Server, ServerBasicDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ServerId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ServerName))
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.ServerMembers))
            .ReverseMap();
        CreateMap<Server, ServerDetailsDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ServerId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ServerName))
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.ServerMembers))
            .ReverseMap();
        CreateMap<ServerBan, BanDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BanId))
            .ReverseMap();
        CreateMap<ServerMember, ServerMemberDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Member.Id))
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Member.DisplayName))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Member.Avatar))
            .ForMember(dest => dest.CustomStatus, opt => opt.MapFrom(src => src.Member.CustomStatus))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Member.Status))
            .ReverseMap();
        CreateMap<ServerRole, ServerRoleDto>()
            .ReverseMap();
    }
}