using AutoMapper;
using AutoNova.API.DTOs.Response;
using AutoNova.Domain.Entities;

namespace AutoNova.API.Mapping;

public class MaintenanceHistoryMappingProfile : Profile
{
    public MaintenanceHistoryMappingProfile()
    {
        CreateMap<MaintenanceHistory, MaintenanceHistoryResponse>()
            .ForMember(dest => dest.Plate,        opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Plate : string.Empty))
            .ForMember(dest => dest.Brand,        opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Brand : string.Empty))
            .ForMember(dest => dest.Model,        opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Model : string.Empty))
            .ForMember(dest => dest.OwnerName,    opt => opt.MapFrom(src => src.Vehicle != null && src.Vehicle.Owner != null ? src.Vehicle.Owner.FullName : string.Empty))
            .ForMember(dest => dest.MechanicName, opt => opt.MapFrom(src => src.Mechanic != null ? src.Mechanic.Name : string.Empty));
    }
}
