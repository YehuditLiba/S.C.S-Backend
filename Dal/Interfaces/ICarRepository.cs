using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal.DataObject;

namespace Dal.Interfaces
{
    public interface ICarRepository:IRepository<Car>
    {
        public Task<bool> ChangeTheCarModeAsync(int carId);
        Task<Car> GetByNameAsync(string carName);
        Task<Car> ReadByNameAsync(string carName);
        Task<List<Car>> GetAvailableCarsByStationIdAsync(int stationId);
    }
}
