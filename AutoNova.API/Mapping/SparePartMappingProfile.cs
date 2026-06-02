using AutoMapper;
using AutoNova.API.DTOs.Response;
using AutoNova.Domain.Entities;

namespace AutoNova.API.Mapping;

public class SparePartMappingProfile : Profile
{
    public SparePartMappingProfile()
    {
        CreateMap<SparePart, SparePartResponse>()
            .ForMember(dest => dest.IsLowStock, opt => opt.MapFrom(src => src.Quantity < src.MinimumStock));
    }
}
