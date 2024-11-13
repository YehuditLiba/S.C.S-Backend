using BL.DTO;
using BL.Implementation;
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
        [Route("{getAll}")]
        public async Task<List<StationDTO>> GetAllAsync(string getAll = null)
        {
            return await stationService.ReadAllAsync();
        }

        [HttpPut]
        public async Task<bool> UpdateAsync(StationDTO stationDTO)
        {
            return await stationService.UpdateAsync(stationDTO);
        }
        [HttpDelete]
        public async Task<bool> DeleteAsync(int id)
        {
            return await stationService.DeleteAsync(id);
        }
        [HttpPost("GetNearestStation")]
        public async Task<ActionResult<StationDTO>> GetNearestStation([FromBody] StationDTO stationDTO)
        {
            if (stationDTO == null || stationDTO.X == 0 || stationDTO.Y == 0)
            {
                return BadRequest("הקואורדינטות לא תקינות.");
            }

            StationDTO nearestStation = await stationService.GetNearestStation(stationDTO.X, stationDTO.Y);

            if (nearestStation == null)
            {
                return NotFound("לא נמצאה תחנה קרובה.");
            }

            return Ok(nearestStation); 
        }

        [HttpPost("GetLucrativeStation")]
      
        public async Task<IActionResult> GetLucrativeStation([FromBody] StationDTO stationDTO)
        {
         
            var station = await stationService.GetLucrativeStation(
                numberOfRentalDays: 5,  // מספר ימי השכרה
                x: stationDTO.X, // קואורדינטת X
                y: stationDTO.Y  // קואורדינטת Y
            );

            if (station == null)
            {
                return NotFound();
            }

            return Ok(station);
        }


    }
}
