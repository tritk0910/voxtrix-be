using Application.DTOs.Message;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class MessageProfile : Profile
{
    public MessageProfile()
    {
        CreateMap<Message, DirectMessageDto>()
            .ReverseMap();
        CreateMap<Message, MessageDto>()
            .ReverseMap();
    }
}