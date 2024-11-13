using Dal.DataObject;
using Dal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.Implemention
{
    internal class StationRepository : IStationRepository
    {
        private readonly General general;

        public StationRepository(General general)
        {
            this.general = general;
        }

        #region basic-CRUD

        public async Task<int> CreateAsync(Station station)
        {
            var newStation = general.Stations.Add(station);
            await general.SaveChangesAsync();
            return newStation.Entity.Id;
        }

        public async Task<bool> DeleteAsync(int stationId)
        {
            try
            {
                Station? station = general.Stations.Find(stationId);
                if (station != null)
                {
                    general.Stations.Remove(station);
                    await general.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<List<Station>> ReadAllAsync()
        {
            return await general.Stations.Include(station => station.Street)
                .ThenInclude(street => street.Neigborhood)
                .ThenInclude(nei => nei.City)
                .ToListAsync<Station>();
        }
        public async Task<Station?> ReadByIdAsync(int code)
        {
            return await general.Stations.FindAsync(code);
        }
        public async Task<bool> UpdateAsync(Station newItem)
        {
            try
            {
                general.Stations.Update(newItem);
                await general.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        public async Task<Station> GetNearestStation(double x, double y)
        {
            // שליפת כל התחנות המלאות בלבד
            List<Station> stationList = await general.Stations
                .Where(st => st.IsFull == true)
             //   .Include(station=>station.Id)
                .Include(station => station.Street)
                .ThenInclude(street => street.Neigborhood)
                .ThenInclude(nei => nei.City)
                .ToListAsync();

            if (stationList == null || !stationList.Any())
                return null;

            // מציאת התחנה הקרובה ביותר מתוך הרשימה המסוננת
            return await FindNearestStationFromList(x, y, stationList);
        }
        private async Task<Station> FindNearestStationFromList(double x, double y, List<Station> stationList)
        {
            Station nearestStation = new Station();
            double distance, minDistance = double.MaxValue;
            Point point2;

            foreach (var st in stationList)
            {
                point2 = new Point() { X = st.X, Y = st.Y };
                distance = GetDistance(new Point() { X = x, Y = y }, point2);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    nearestStation = st;
                }
            }

            return nearestStation;
        }
        private static double GetDistance(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2));
        }
        public async Task<List<Station>> GetAllStations()
        {
            return await general.Stations
                .Include(station => station.Street)          // כולל את פרטי הרחוב
                .ThenInclude(street => street.Neigborhood)   // כולל את פרטי השכונה
                .ThenInclude(nei => nei.City)                // כולל את פרטי העיר
                .ToListAsync<Station>();
        }
        public async Task<bool> IsStationFullAsync(int stationId)
        {
            // 1. חיפוש כל הרכבים השייכים לתחנה
            var stationCars = await general.StationToCars
                .Where(stc => stc.StationId == stationId)
                .ToListAsync();

            // 2. אם אין רכבים תחנה לא נחשבת כמלאה
            if (!stationCars.Any())
            {
                return false;
            }

            // 3. בדיקה אם כל הרכבים לא פנויים
            bool isFull = stationCars.All(stc => stc.Car.Status != CarStatus.Available);

            return isFull; // מחזירים true אם התחנה מלאה
        }
        public async Task<Station?> ReadByIdWithCarsAsync(int stationId)
        {
            return await general.Stations
                .Include(st => st.Street)
                .ThenInclude(street => street.Neigborhood)
                .ThenInclude(nei => nei.City)
                .Include(st => st.StationToCars) // כולל את הקשר של התחנה לרכבים
                .ThenInclude(stc => stc.Car) // כולל את המידע על הרכב
                .FirstOrDefaultAsync(st => st.Id == stationId);
        }
        public async Task<Station> ReadByNumberAsync(int number)
        {
            return await general.Stations
                .FirstOrDefaultAsync(s => s.Number == number);
        }

    }
}
