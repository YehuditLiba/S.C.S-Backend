using AutoMapper;
using BL.DTO;
using BL.Interfaces;
using Dal.DataObject;
using Dal.Implemention;
using Dal.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace BL.Implementation
{
    public class StationService : IStationService
    {
        IStationRepository stationRepository;
        private readonly ICarRepository _carRepository;
        IMapper mapper;

        public StationService(IStationRepository stationRepository, IMapper mapper, ICarRepository carRepository)
        {
            this._carRepository = carRepository;
            this.stationRepository = stationRepository;
            this.mapper = mapper;
        }

        #region basic CRUD

        public async Task<int> CreateAsync(StationDTO item)
        {
            Station station = mapper.Map<Station>(item);
            return await stationRepository.CreateAsync(station);
        }

        public async Task<List<StationDTO>> ReadAllAsync()
        {
            List<Station> lst = await stationRepository.ReadAllAsync();
            return mapper.Map<List<StationDTO>>(lst);
        }

        public async Task<StationDTO> ReadByIdAsync(int id)
        {
            Station station = await stationRepository.ReadByIdAsync(id);
            return mapper.Map<StationDTO>(station);
        }

        public async Task<bool> UpdateAsync(StationDTO stationDTO)
        {
            Station newStation = mapper.Map<Station>(stationDTO);
            return await stationRepository.UpdateAsync(newStation);
        }

        public async Task<bool> DeleteAsync(int code)
        {
            return await stationRepository.DeleteAsync(code);
        }

        #endregion
        public async Task<StationDTO> GetNearestStation(double x, double y)
        {
            // חיפוש תחנה קרובה שהיא מלאה
            Station station = await stationRepository.GetNearestStation(x, y);

            if (station == null)
            {
                throw new Exception("לא נמצאה תחנה קרובה עם רכב זמין");
            }

            // שליפת התחנה יחד עם רשימת הרכבים שלה
            var stationWithCars = await stationRepository.ReadByIdWithCarsAsync(station.Id);

            if (stationWithCars == null)
            {
                throw new Exception("לא נמצאו רכבים קשורים לתחנה");
            }

            // החזרת תחנה כ-DTO
            StationDTO stationDTO = mapper.Map<StationDTO>(stationWithCars);

            // שליפת פרטי הרחוב, השכונה והעיר מתוך התחנה
            stationDTO.Street = stationWithCars.Street?.Name;
            stationDTO.Neighborhood = stationWithCars.Street?.Neigborhood?.Name;
            stationDTO.City = stationWithCars.Street?.Neigborhood?.City?.Name;

            stationDTO.CarNames = stationWithCars.StationToCars
                .Where(stc => stc.Car?.Status == Dal.DataObject.CarStatus.Available) // השתמש במפורש ב-namespace של Dal.DataObject
                .Select(stc => stc.Car?.Name)
                .Where(name => name != null)
                .ToList();








            return stationDTO;
        }

        public async Task<StationDTO> GetLucrativeStation(int numberOfRentalDays, double x, double y)
        {
            //קריטריונים
            double discount = PriceDetermination.Discount();
            double pricePerDay = PriceDetermination.Price_per_day();
            double normalWalkingDistanceInKm = PriceDetermination.Normal_wolking_distance_in_km();
            double avgTaxiFarePerKm = PriceDetermination.Avg_price_of_taxi_fare_for_km();
            double initialTaxiFare = PriceDetermination.Initial_state_of_counter();

            Point userLocation = new Point { X = x, Y = y };

            Station nearestCentralStation = await stationRepository.GetNearestStation(userLocation.X, userLocation.Y);
            if (nearestCentralStation == null)
                return null;

            // חישוב מרחק
            double distanceToCentralStation = GetDistance(userLocation, new Point { X = nearestCentralStation.X, Y = nearestCentralStation.Y });

            if (distanceToCentralStation <= normalWalkingDistanceInKm)
            {
                return await GetStationDetails(nearestCentralStation);
            }
            // taxi
            else if (initialTaxiFare + (distanceToCentralStation * avgTaxiFarePerKm) <= discount * numberOfRentalDays * pricePerDay)
            {
                return await GetStationDetails(nearestCentralStation);
            }
            else
            {
                Station nearestStation = await stationRepository.GetNearestStation(userLocation.X, userLocation.Y);
                return await GetStationDetails(nearestStation);
            }
        }
        // DTO 
        private async Task<StationDTO> GetStationDetails(Station station)
        {
            var street = station.Street;
            var neighborhood = street?.Neigborhood;
            var city = neighborhood?.City;

            if (street == null || neighborhood == null || city == null)
                return null;

            StationDTO stationDTOResult = mapper.Map<StationDTO>(station);
            stationDTOResult.Street = street.Name;
            stationDTOResult.Neighborhood = neighborhood.Name;
            stationDTOResult.City = city.Name;

            return stationDTOResult;
        }

        private static double GetDistance(Point point1, Point point2)
        {
            double R = 6371; // Radius of the earth in km
            double dLat = ToRadians(point2.Y - point1.Y);  // Degrees to radians
            double dLon = ToRadians(point2.X - point1.X);
            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(point1.Y)) * Math.Cos(ToRadians(point2.Y)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = R * c; // Distance in km
            return distance;
        }

        private static double ToRadians(double angle)
        {
            return angle * (Math.PI / 180);
        }

        const string API_KEY = "AIzaSyBFwHxGY47K0J1ECt99_TZA7aVO62ztUp0"; // Replace with actual API key
        const string BASE_URL = "https://maps.googleapis.com/maps/api/geocode/json";

        private async Task<Point> ConvertStationDTOToPointAsync(StationDTO stationDTO)
        {
            if (string.IsNullOrEmpty(stationDTO.Street) || string.IsNullOrEmpty(stationDTO.City))
            {
                throw new ArgumentException("Street or City information is missing.");
            }

            Point point = new Point();
            string country = "IL"; // Assuming Israel, adjust if needed
            string url = $"{BASE_URL}?address={Uri.EscapeDataString(stationDTO.Street)} {Uri.EscapeDataString(stationDTO.City)}&components=country:{country}&key={API_KEY}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode(); // Ensure the request succeeded (status code 2xx)

                    string responseJson = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(responseJson);

                    // Check if we have results and geometry
                    if (result.status == "OK" && result.results.Count > 0)
                    {
                        var firstResult = result.results[0];

                        // Ensure we have geometry and location data before accessing
                        if (firstResult.geometry != null && firstResult.geometry.location != null)
                        {
                            point.X = firstResult.geometry.location.lat;
                            point.Y = firstResult.geometry.location.lng;
                        }
                        else
                        {
                            throw new Exception("No geometry or location data available.");
                        }

                        // Safely access address_components
                        if (firstResult.address_components != null && firstResult.address_components.Count > 1)
                        {
                            stationDTO.Street = firstResult.address_components[0]?.short_name ?? "Unknown Street";
                            stationDTO.City = firstResult.address_components[1]?.short_name ?? "Unknown City";
                        }
                        else
                        {
                            throw new Exception("Not enough address components returned.");
                        }

                        // Assume no neighborhood info
                        stationDTO.Neighborhood = null;
                    }
                    else
                    {
                        throw new Exception("No valid results found in the API response.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception (log it, throw custom error, etc.)
                throw new Exception($"Error converting station DTO to point: {ex.Message}");
            }

            return point;
        }

        public async Task<List<Car>> GetAvailableCarsInFullStationAsync(int stationId)
        {
            // 1. בדוק אם התחנה מלאה
            bool isStationFull = await stationRepository.IsStationFullAsync(stationId);

            // 2. אם התחנה מלאה, החזר את כל הרכבים הפנויים
            if (isStationFull)
            {
                return await _carRepository.GetAvailableCarsByStationIdAsync(stationId);
            }

            // אם התחנה לא מלאה, מחזירים רשימה ריקה
            return new List<Car>();
        }
        public async Task<StationDTO> ReadByIdWithCarsAsync(int stationId)
        {
            Station station = await stationRepository.ReadByIdWithCarsAsync(stationId);
            if (station == null)
                return null;

            //DTO
            StationDTO stationDTO = mapper.Map<StationDTO>(station);

            //DTO
            stationDTO.CarNames = station.StationToCars
                .Select(stc => stc.Car.Name)
                .ToList();

            return stationDTO;
        }

    }
}
