using AutoMapper;
using TallerAutonova.API.DTOs.Request;
using TallerAutonova.API.DTOs.Response;
using TallerAutonova.Domain.Entities;

namespace TallerAutonova.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Team mappings
            CreateMap<MaintenanceHistoryRequestDTO, MaintenanceHistory>();
            CreateMap<MaintenanceHistory, MaintenanceHistoryResponseDTO>()
                .ForMember(dest => dest.MechanicName, // En el DTO, la propiedad TournamentName se llena con Tournament.Name del objeto original
                    opt => opt.MapFrom(src => src.Mechanic.Name))
                .ForMember(dest => dest.VehiclePlate,
                    opt => opt.MapFrom(src => src.Vehicle.Plate))
                //.ForMember(dest => dest.ReceptionistName,
                //    opt => opt.MapFrom(src => src.Receptionist.Name))
                ;

            CreateMap<AppointmentRequestDTO, Appointment>();
            CreateMap<Appointment, AppointmentResponseDTO>()
                .ForMember(
                    dest => dest.State,
                    opt => opt.MapFrom(src =>
                    src.State.ToString()))
                .ForMember(
                    dest => dest.VehicleId,
                    opt => opt.MapFrom(src =>
                    src.Vehicle.Id))
                .ForMember(
                    dest => dest.MechanicId,
                    opt => opt.MapFrom(src =>
                    src.Mechanic.Id)); ;

            CreateMap<VehicleRequestDTO, Vehicle>();
            CreateMap<Vehicle, VehicleResponseDTO>()
                .ForMember(
                    dest => dest.OwnerName,
                    opt => opt.MapFrom(src => src.Owner.Name))
                .ForMember(
                    dest => dest.OwnerPhone,
                    opt => opt.MapFrom(src => src.Owner.Phone));
        }
    }
}