using Application.DTOs.Channels;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class ChannelProfile : Profile
{
    public ChannelProfile()
    {
        CreateMap<Channel, ChannelDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ChannelId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ChannelName))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.ChannelType))
            .ReverseMap();
        CreateMap<Channel, CreateChannelDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ChannelName))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.ChannelType))
            .ReverseMap();
        CreateMap<Channel, UpdateChannelDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ChannelName))
            .ReverseMap();
    }
}