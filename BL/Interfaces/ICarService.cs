using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTO;
using Dal.DataObject;

namespace BL.Interfaces
{
    public interface ICarService:IService<CarDTO>
    {
        public Task<bool> ChangeTheCarModeAsync(int carId);
        Task<CarDTO> ReadByNameAsync(string carName);
        Task<bool> RentCarAsync(int carId, int stationId);
        Task<bool> ReturnCarAsync(string carName, int statiId);



    }
}
