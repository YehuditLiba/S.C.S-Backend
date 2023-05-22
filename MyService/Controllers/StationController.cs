using BL.DTO;
using BL.Interfaces;
using BL.profiles;
using Dal.DataObject;
using Microsoft.AspNetCore.Mvc;

namespace MyService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StationController : ControllerBase
    {
        IStationService stationService;
        public StationController(IStationService stationService)
        {
            this.stationService = stationService;
        }
        [HttpGet]
        public async Task<Station> GetNearestStation (int num,string street,string neighborhood, string city)
        {
            StationAndStationDTO stationDTO= new StationAndStationDTO();
            StationDTO station=new StationDTO(num, street, neighborhood, city);
            return await stationService.GetNearestStation (station);
        }
    }
}
