using Application.DTOs.Servers.Roles;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Role, RoleDto>()
            .ReverseMap();
        CreateMap<Role, RoleDetailsDto>()
            .ForMember(dest => dest.MemberCount, opt => opt.MapFrom(src => src.ServerMemberRoles.Count))
            .ReverseMap();
        CreateMap<CreateRoleDto, Role>()
            .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Permissions.ToLong())); // Convert enum to long
        CreateMap<UpdateRoleDto, Role>()
            .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Permissions.ToLong())); // Convert enum to long
        CreateMap<long, RolePermission>().ConvertUsing(src => RolePermissionHelper.FromLong(src)); // Convert long to enum if needed
    }
}