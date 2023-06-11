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
            //try
            //{
            //    Car car = general.Cars.Find(carId);
            //    general.Cars.RemoveRange(car);
            //   // general.StationToCars.Remove();
            //    await general.SaveChangesAsync();
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
            try
            {
                // Step 1: Update the StationToCar table to set CarId to NULL
                var stationToCarEntries = general.StationToCars.Where(stc => stc.CarId == carId);
                foreach (var entry in stationToCarEntries)
                {
                    entry.CarId = null;
                }

                // Step 2: Remove the car from the Cars table
                Car car = general.Cars.Find(carId);
                general.Cars.Remove(car);

                await general.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

       public async Task<List<Car>> ReadAllAsync()
        {
            return await general.Cars.ToListAsync<Car>();
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
