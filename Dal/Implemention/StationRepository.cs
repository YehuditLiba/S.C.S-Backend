using Dal.DataObject;
using Dal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return await general.Stations.ToListAsync<Station>();

        }

        public async Task<Station> ReadByIdAsync(int code)
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

        public Station GetNearestStation(Station station)
        {
            var address = "Stavanger, Norway";

            throw new NotImplementedException();
        }
    }
}
