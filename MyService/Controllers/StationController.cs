using BL.DTO;
using BL.Interfaces;
using BL.profiles;
using Dal.DataObject;
using Microsoft.AspNetCore.Mvc;

namespace MyService.Controllers
{
    public class StationController : ServiceController
    {
        IStationService stationService;
        public StationController(IStationService stationService)
        {
            this.stationService = stationService;
        }
        [HttpGet]
        public async Task<StationDTO> GetNearestStation(int num, string street, string neighborhood, string city)
        {
            StationDTO stationDTO = new StationDTO(num, street, neighborhood, city);
            return await stationService.GetNearestStation(stationDTO);
        }
        [HttpGet]
        [Route("{numOfRentalHours}")]
        public async Task<StationDTO> FindLucrativeStation(int numOfRentalHours, [FromBody] int num, string street, string neighborhood, string city)
        {
            StationDTO stationDTO = new StationDTO(num, street, neighborhood, city);
            return await stationService.GetLucrativeStation(numOfRentalHours, stationDTO);
        }
    }
}

