using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal.DataObject;
using Dal.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            var newCar = general.Car.Add(item);
            await general.SaveChangesAsync();
            return newCar.Entity.Id;
        }

        public async Task<bool> DeleteAsync(int carId)
        {
            try
            {
                // Step 1: Ensure the car exists before attempting to delete
                Car car = await general.Car.FindAsync(carId);
                if (car == null)
                {
                    return false;  // Return false if the car does not exist
                }

                // Step 2: Update the StationToCar table to set CarId to NULL
                var stationToCarEntries = general.StationToCars.Where(stc => stc.CarId == carId);
                foreach (var entry in stationToCarEntries)
                {
                    entry.CarId = null;
                }

                // Step 3: Remove the car from the Cars table
                general.Car.Remove(car);

                await general.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting car with ID {carId}: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Car>> ReadAllAsync()
        {
            return await general.Car.ToListAsync<Car>();
        }
        public async Task<Car> ReadByIdAsync(int code)
        {
            return await general.Car.FirstAsync(c => c.Id == code);
        }

        public Task<bool> UpdateAsync(Car newItem)
        {
            //details of car will never change
            throw new NotImplementedException();
        }
        public async Task<bool> ChangeTheCarModeAsync(int carId)
        {
            try
            {
                // Find the car in the database
                Car car = await general.Car.FirstOrDefaultAsync(c => c.Id == carId);

                if (car != null)
                {
                    // Toggle the car status
                    switch (car.Status)
                    {
                        case CarStatus.Available:
                            car.Status = CarStatus.Taken;
                            break;
                        case CarStatus.Taken:
                            car.Status = CarStatus.Returned;
                            break;
                        case CarStatus.Returned:
                            car.Status = CarStatus.Available;
                            break;
                        case CarStatus.Reserved:
                            car.Status = CarStatus.Available;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("Unknown car status");
                    }

                    await general.SaveChangesAsync();
                    return true; // Success
                }
                else
                {
                    Console.WriteLine($"Car with ID {carId} not found.");
                    return false; // Car does not exist
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error changing status for car with ID {carId}: {ex.Message}");
                return false; // Failure
            }
        }
        public async Task<Car> GetByNameAsync(string carName)
        {
            return await general.Car
                .FirstOrDefaultAsync(c => c.Name == carName);  // מחפש את הרכב לפי שם
        }
        public async Task<Car> ReadByNameAsync(string carName)
        {
            return await general.Car
                .FirstOrDefaultAsync(c => c.Name == carName); // אם צריך, אפשר להוסיף לוגיקה נוספת
        }

    }
}
