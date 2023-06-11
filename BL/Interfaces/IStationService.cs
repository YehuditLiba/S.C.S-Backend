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
        public Task<StationDTO> GetNearestStation(StationDTO stationDTO);
        public Task<StationDTO> FindLucrativeStation(int numOfRentalHours, StationDTO stationDTO);
    }
}
