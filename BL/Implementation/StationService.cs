using AutoMapper;
using BL.DTO;
using BL.Interfaces;
using Dal.DataObject;
using Dal.Interfaces;
using GoogleMaps.LocationServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BL.Implementation
{
    public class StationService : IStationService
    {
        IStationRepository stationRepository;
        IMapper mapper;
        public StationService(IStationRepository stationRepository, IMapper mapper)
        {
            this.stationRepository = stationRepository;
            this.mapper = mapper;
        }
        #region basic CRUD

        public Task<int> CreateAsync(StationDTO item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int code)
        {
            throw new NotImplementedException();
        }

        public Task<List<StationDTO>> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<StationDTO> ReadByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(StationDTO newItem)
        {
            throw new NotImplementedException();
        }
        #endregion''' 


        public async Task<StationDTO> GetNearestStation(StationDTO stationDTO)
        {
            //I get stationDTO, convert it to a point, find the nearest station and return it as stationDTO
            Point point = convertStationDTOToPoint(stationDTO);
            //convert:

            Station station = await stationRepository.GetNearestStation(point, stationDTO.Street, stationDTO.Neighborhood, stationDTO.City);
            return mapper.Map<StationDTO>(station);
        }

        public async Task<Station> FindLucrativeStation(int numberOfRentalHours, StationDTO stationDTO)
        {
            //get the nearest station and check: if it centeral-well, return it
            //if not find the lucrative station and return it 
            //Station station = mapper.Map<Station>(stationDTO);
            //הלוגיקה צריכה להיות כאן!!!
            //אם פחות מ??? ק"מ - אפשר ללכת רגלית
            //אם אפילו נסיעה במונית יוצאת זול יותר מאשר לותר על ההנחה
            throw new NotImplementedException();
        }
        const string API_KEY = "AIzaSyBFwHxGY47K0J1ECt99_TZA7aVO62ztUp0";
        const string BASE_URL = "https://maps.googleapis.com/maps/api/geocode/json";
        private Point convertStationDTOToPoint(StationDTO stationDTO)
        {
            StationDTO locationFromGoogleMaps;
            Point point = new Point();
            string country = "IL";
            string url = BASE_URL + "?address=" + stationDTO.Street + "st" + stationDTO.City + "&components==country:" + country + "&key=" + API_KEY;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseJson = reader.ReadToEnd();
                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(responseJson);
                if (result.status == "OK")
                {
                    point.X = result.results[0].geometry.bounds.northeast.lat;
                    point.Y = result.results[0].geometry.bounds.northeast.lng;
                    //the next lines will change the object stationDTO, because the function got it as a reference type

                    //stationDTO.Number=result.results[0].geometry.number;
                    //stationDTO.Street = result.results[0].geometry.bounds.northeast;
                    //stationDTO.Neighborhood = result.results[0].geometry.bounds.northeast;
                    //stationDTO.City = result.results[0].geometry.bounds.northeast;

                    //locationFromGoogleMaps = new StationDTO (result[1]???????????,result[0].?????,???,???);
                }
            }
            catch (WebException)
            {
                //until our seminary will open google maps service:)
                point.X = 3.333;
                point.Y = 3.333;
                stationDTO.Number = 51;
                stationDTO.Street = "Chazon-Ish";
                stationDTO.Neighborhood = "Ramat-Shlomo";
                stationDTO.City = "Jerusalem";
            }
            return point;
        }
        private StationDTO convertPointToStationDTO(Point point)
        {
            Station station = new Station();
            return null;

        }

        //Task<Station> IStationService.GetNearestStation(StationDTO stationDTO)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
