using Application.DTOs.Servers;
using Application.DTOs.Servers.Roles;
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
        CreateMap<Server, ServerTransferDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ServerId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ServerName))
            .ReverseMap();
        CreateMap<CreateServerDto, Server>()
            .ForMember(dest => dest.ServerName, opt => opt.MapFrom(src => src.Name))
            .ReverseMap();
        CreateMap<EditServerDto, Server>()
            .ForMember(dest => dest.ServerName, opt => opt.MapFrom(src => src.Name))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<Server, ServerBasicDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ServerId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ServerName))
            .ReverseMap();
        CreateMap<Server, ServerDetailsDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ServerId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ServerName))
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.ToList()))
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
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.ServerMemberRoles.Select(x => x.Role)))
            .ReverseMap();
    }
}