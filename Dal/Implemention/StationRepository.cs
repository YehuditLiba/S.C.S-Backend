using Dal.DataObject;
using Dal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = Dal.DataObject.Point;

namespace Dal.Implemention
{
    internal class StationRepository : IStationRepository
    {
        General general;
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
        //public async Task<List<Station>> GetByStreetId(int streetId)
        //{
        //    return await general.Stations.Where(s => (s.StationToCars.Where(stc => stc.StationId == s.Id && stc.CarId != null) && s.StreetId == streetId)).ToListAsync();
        //}
        public async Task<List<Station>> GetByNeighborhoodId(int streetId)
        {
            return await general.Stations.Where(s => s.StreetId == streetId && s.IsCenteral.Value).ToListAsync();
        }
        public async Task<List<Station>> GetByCityId(int streetId)
        {
            return await general.Stations.Where(s => s.StreetId == streetId && s.IsCenteral.Value).ToListAsync();
        }
        public async Task<Station> GetNearestStation(Point point1, string street, string neighbornhood, string city)
        {

            List<Station> stationList;
            stationList = await ReadAllAsync();
            Station nearestStation = new();
            double distance, minDistance = double.MaxValue;
            foreach (var st in stationList)
            {
                Point point2=new Point() { X=st.X,Y=st.Y};
                distance = GetDistance(point1,point2);
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
            return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }
    }
}
