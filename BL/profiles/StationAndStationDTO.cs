using AutoMapper;
using BL.DTO;
using Dal.DataObject;

namespace BL.profiles
{
    public class StationAndStationDTO : Profile
    {
        public StationAndStationDTO()
        {
            CreateMap<Station, StationDTO>()
                .ForMember(dto => dto.Id, o => o.MapFrom(s => s.Id))
                .ForMember(x => x.X, o => o.MapFrom(s => s.X))
                .ForMember(y => y.Y, o => o.MapFrom(s => s.Y))
                .ForMember(dto => dto.Number, o => o.MapFrom(s => s.Number.HasValue ? s.Number.Value : 0)) 
                .ForMember(dto => dto.Street, o => o.MapFrom(s => s.Street.Name)) 
                .ForMember(dto => dto.City, o => o.MapFrom(s => s.Street.Neigborhood.City.Name)) 
                .ForMember(dto => dto.Neighborhood, o => o.MapFrom(s => s.Street.Neigborhood.Name))
                .ConstructUsing((src, ctx) => new StationDTO(
                    src.Id,
                    src.Number.HasValue ? src.Number.Value : 0, // Nullable
                    src.Street.Name,
                    src.Street.Neigborhood.City.Name,
                    src.Street.Neigborhood.Name,
                    src.X,
                    src.Y)); 
        }
    }
}
