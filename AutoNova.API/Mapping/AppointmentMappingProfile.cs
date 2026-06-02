using AutoMapper;
using AutoNova.API.DTOs.Response;
using AutoNova.Domain.Entities;

namespace AutoNova.API.Mapping;

public class AppointmentMappingProfile : Profile
{
    public AppointmentMappingProfile()
    {
        CreateMap<Appointment, AppointmentResponse>()
            .ForMember(dest => dest.Status,       opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Plate,        opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Plate : string.Empty))
            .ForMember(dest => dest.VehicleBrand, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Brand : string.Empty))
            .ForMember(dest => dest.VehicleModel, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Model : string.Empty))
            .ForMember(dest => dest.OwnerName,    opt => opt.MapFrom(src => src.Vehicle != null && src.Vehicle.Owner != null ? src.Vehicle.Owner.FullName : string.Empty))
            .ForMember(dest => dest.MechanicName, opt => opt.MapFrom(src => src.Mechanic != null ? src.Mechanic.Name : null));
    }
}
