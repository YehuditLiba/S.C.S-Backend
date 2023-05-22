using AutoMapper;
using BL.DTO;
using Dal.DataObject;
using GoogleMaps.LocationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.profiles
{
    public class StationAndStationDTO : Profile
    {
        public StationAndStationDTO()
        {
            GetLocation(new StationDTO(21,"mintz", "ramot", "jerusalem")) ;
            //   CreateMap<StationDTO, Station>().ForMember(sDTO=>getStation(sDTO));
        }
        private void GetLocation(StationDTO stationDTO)
        {
            string address = stationDTO.Street + stationDTO.Number + stationDTO.Neighborhood + stationDTO.City + "Israel";
            string apiKey = "AIzaSyBFwHxGY47K0J1ECt99_TZA7aVO62ztUp0";
            var locationService = new GoogleLocationService(apiKey);
            var point = locationService.GetLatLongFromAddress(address);

            var latitude = point.Latitude;
            var longitude = point.Longitude;
        }

    }

}
//CreateMap<Book, BookDTO>()
//    .ForMember(
//    bDto => bDto.Author,
//    options => options.MapFrom(b => b.Author == null ? "" : b.Author.FirstName + " " + b.Author.LastName))
//    .ReverseMap()
//    .ForMember(b => b.Category, opts => opts.Ignore())
//    .ForPath(b => b.Author.FirstName, opts =>         opts.MapFrom(b => b.Author.Split(" ", System.StringSplitOptions.None)[0]))
//    .ForPath(b => b.Author.LastName, opts => opts.MapFrom(b => b.Author.Split(" ", System.StringSplitOptions.None)[1]));