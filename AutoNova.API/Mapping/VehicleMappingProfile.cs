using AutoMapper;
using AutoNova.API.DTOs.Response;
using AutoNova.Domain.Entities;

namespace AutoNova.API.Mapping;

public class VehicleMappingProfile : Profile
{
    public VehicleMappingProfile()
    {
        CreateMap<Owner, OwnerResponse>();

        CreateMap<Vehicle, VehicleResponse>()
            .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner));
    }
}
