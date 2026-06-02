using AutoMapper;
using AutoNova.API.DTOs.Response;
using AutoNova.Domain.Entities;

namespace AutoNova.API.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserResponse>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
    }
}
