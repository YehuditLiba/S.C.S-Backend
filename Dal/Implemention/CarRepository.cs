using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal.DataObject;
using Dal.Interfaces;

namespace Dal.Implemention
{
    public class CarRepository : ICarRepository
    {
        General general;
        public CarRepository(General general)
        {
            this.general = general;
        }
        public async Task<int> CreateAsync(Car item)
        {
            var newCar = general.Cars.Add(item);
            await general.SaveChangesAsync();
            return newCar.Entity.Id;
        }

        public async Task<bool> DeleteAsync(int carId)
        {
            try
            {
                Car car = general.Cars.Find(carId);
                if (car != null)
                {
                    general.Cars.Remove(car);
                    await general.SaveChangesAsync();
                }
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        public Task<List<Car>> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Car> ReadByIdAsync(int code)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Car newItem)
        {
            throw new NotImplementedException();
        }
    }
}
