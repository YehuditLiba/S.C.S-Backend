using BL.DTO;
using Dal.DataObject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Interfaces
{
    public interface IStationService
    {
        public Task<Station> GetNearestStation(StationDTO stationDTO);
        public Task<Station> FindLucrativeStation(int numOfRentalHours, StationDTO stationDTO);
    }
}
