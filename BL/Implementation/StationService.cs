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

        //this public function return the nearest stationDTO:
        public async Task<StationDTO> GetNearestStation(StationDTO stationDTO)
        {
            Station station = await FindNearestStation(stationDTO);
            return mapper.Map<StationDTO>(station);
        }

        //this private function return the nearest station:
        private async Task<Station> FindNearestStation(StationDTO stationDTO)
        {
            Point point = convertStationDTOToPoint(stationDTO);
            return await stationRepository.GetNearestStation(point, stationDTO.Street, stationDTO.Neighborhood, stationDTO.City);
        }

        public async Task<StationDTO> FindLucrativeStation(int numberOfRentalHours, StationDTO stationDTO)
        {
            const double NORMAL_WALKINK_DISTANCE_IN_KM = 1.00;
            const double AVERAGE_PRICE_OF_TAXI_FARE_FOR_KM = 13.5;
            Station lucrativeStation = new Station();
            //if the nearest station is centeral - this is the lucrative Station, so return it:
            Station nearestStation = await FindNearestStation(stationDTO);
            if (nearestStation.IsCenteral.Value == true)
            {
                lucrativeStation = nearestStation;
            }
            //if not , find the nearest centeral station and check:
            else
            {
                Point point = convertStationDTOToPoint(stationDTO);
                Station nearestCenteralStation = await stationRepository.GetNearestCenteralStation(point, stationDTO.Street, stationDTO.Neighborhood, stationDTO.City);
                //if it within walking distance - return it
                double distance = (double)GetDistance(point, new Point() { X = nearestCenteralStation.X, Y = nearestCenteralStation.Y });
                if (distance <= NORMAL_WALKINK_DISTANCE_IN_KM)
                {
                    lucrativeStation = nearestCenteralStation;
                }
                //or if average taxi fare is less than the discount - return it
                else if (distance * AVERAGE_PRICE_OF_TAXI_FARE_FOR_KM <= (double)numberOfRentalHours)
                {
                    lucrativeStation = nearestCenteralStation;
                }
                //else - return the nearest station.
                else
                {
                    lucrativeStation = nearestStation;
                }
            }
            return mapper.Map<StationDTO>(lucrativeStation);
        }
        private static double GetDistance(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2));
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
    }
}
