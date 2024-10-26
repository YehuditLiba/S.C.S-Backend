using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BL.DTO;
namespace MyService.Controllers
{
    public class CarController : ServiceController
    {
        ICarService carService;
        public CarController(ICarService carService)
        {
            this.carService = carService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCar(string name, int LicensePlate, int NumOfSeets, CarStatus status)
        {

            CarDTO carDTO = new CarDTO(name, LicensePlate, NumOfSeets, status);
            int carId = await carService.CreateAsync(carDTO);
            return Ok(carId);
        }

        [HttpDelete]
        public async Task<bool> DeleteCar(int carId)
        {
            return await carService.DeleteAsync(carId);
        }
         [HttpGet]
        public async Task<List<CarDTO>> GetAllCars()
        {

            return await carService.ReadAllAsync();
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<CarDTO> GetCarById(int id)
        {
            return await carService.ReadByIdAsync(id);
        }
        [HttpPut]
        public async Task<bool> ChangeTheCarModeAsync(int carId)
        {
            return await carService.ChangeTheCarModeAsync(carId);
        }
    }
}
