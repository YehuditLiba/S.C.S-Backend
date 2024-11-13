using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BL.DTO;
using Dal.DataObject;

namespace BL.profiles
{
    public class CarAndCarDTO : Profile
    {
        public CarAndCarDTO()
        {
            CreateMap<Car, CarDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status)) // מיפוי Status ל-Status
                .ReverseMap();
          
            CreateMap<Station, StationDTO>()
             .ForMember(dest => dest.CarNames, opt => opt.MapFrom(src => src.StationToCars.Select(car => car.Id.ToString()).ToList())) // המרה ל-Id של רכבים
             .ReverseMap();
            CreateMap<Car, CarDTO>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (Dal.DataObject.CarStatus?)src.Status))  // המרת הסטטוס ל-nullable
            .ReverseMap();


        }
    }
}
