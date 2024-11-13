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
                Car car = await general.Car.FindAsync(carId);
                if (car == null)
                {
                    return false;
                }

                //  Update the StationToCar table to set CarId to NULL
                var stationToCarEntries = general.StationToCars.Where(stc => stc.CarId == carId);
                foreach (var entry in stationToCarEntries)
                {
                    entry.CarId = null;
                }

                //  Remove the car from the Cars table
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
        public async Task<bool> UpdateAsync(Car newItem)
        {
            try
            {
                // 1. חיפוש הרכב לפי מזהה
                Car car = await general.Car.FirstOrDefaultAsync(c => c.Id == newItem.Id);

                // אם הרכב לא נמצא
                if (car == null)
                {
                    return false; // מחזיר false אם לא נמצא רכב עם מזהה זה
                }

                // 2. עדכון פרטי הרכב
                car.Name = newItem.Name;
                car.LicensePlate = newItem.LicensePlate;
                car.NumOfSeets = newItem.NumOfSeets; // עדכון מספר המושבים
                car.Status = newItem.Status;

                // 3. שמירה במסד הנתונים
                await general.SaveChangesAsync();

                return true; // מחזיר true אם העדכון בוצע בהצלחה
            }
            catch (Exception ex)
            {
                // במקרה של שגיאה, הדפס את השגיאה
                Console.WriteLine($"Error updating car with ID {newItem.Id}: {ex.Message}");
                return false; // מחזיר false במקרה של שגיאה
            }
        }
        public async Task<bool> ChangeTheCarModeAsync(int carId)
        {
            Car car = await general.Car.FirstOrDefaultAsync(c => c.Id == carId);

            if (car != null)
            {
                // שינוי סטטוס הרכב לפי הלוגיקה שנדרשת
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

                await general.SaveChangesAsync(); // שמירה במסד הנתונים
                return true;
            }
            else
            {
                Console.WriteLine($"Car with ID {carId} not found.");
                return false; // הרכב לא נמצא
            }
        }
        public async Task<Car> GetByNameAsync(string carName)
        {
            return await general.Car
                .FirstOrDefaultAsync(c => c.Name == carName); 
        }
        public async Task<Car> ReadByNameAsync(string carName)
        {
            return await general.Car
                .FirstOrDefaultAsync(c => c.Name == carName);
        }
        public async Task<bool> RentCarAsync(int carId, int stationId)
        {
            var car = await general.Car.FirstOrDefaultAsync(c => c.Id == carId);
            var station = await general.Stations.FirstOrDefaultAsync(s => s.Id == stationId);
            if (car == null || station == null || car.Status == CarStatus.Taken)
            {
                Console.WriteLine("Car or station not found, or car is already taken.");
                return false;
            }

            //-"Taken"
            car.Status = CarStatus.Taken;

            //  StationToCar
            var stationToCarEntry = await general.StationToCars
                .FirstOrDefaultAsync(stc => stc.StationId == stationId && stc.CarId == carId);

            if (stationToCarEntry != null)
            {
                stationToCarEntry.CarId = null; 
            }
            else
            {
                await general.StationToCars.AddAsync(new StationToCar
                {
                    StationId = stationId,
                    CarId = null
                });
            }
            bool stationIsEmpty = station.StationToCars.All(stc => stc.CarId == null ||
                general.Car.FirstOrDefaultAsync(c => c.Id == stc.CarId).Result.Status == CarStatus.Taken);

            if (stationIsEmpty)
            {
                station.IsFull = false;
            }

            await general.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ReturnCarAsync(string carName, int stationId)
        {
            var car = await general.Car.FirstOrDefaultAsync(c => c.Name == carName);
            var station = await general.Stations.FirstOrDefaultAsync(s => s.Id == stationId);

            //  "Taken" או "Reserved"
            if (car == null || station == null || (car.Status != CarStatus.Taken && car.Status != CarStatus.Reserved))
            {
                return false;
            }

            // "Available"
            car.Status = CarStatus.Available;


            var stationToCarEntry = await general.StationToCars
                .FirstOrDefaultAsync(stc => stc.StationId == stationId && stc.CarId == null);

            if (stationToCarEntry != null)
            {
                stationToCarEntry.CarId = car.Id;
            }
            else
            {
                //  StationToCar
                general.StationToCars.Add(new StationToCar
                {
                    StationId = stationId,
                    CarId = car.Id
                });
            }

            bool stationIsFull = station.StationToCars.All(stc =>
                stc.CarId != null && general.Car.FirstOrDefaultAsync(c => c.Id == stc.CarId).Result.Status == CarStatus.Available);

            if (stationIsFull)
            {
                station.IsFull = true;
            }

            await general.SaveChangesAsync();

            return true;
        }
        public async Task<List<Car>> GetAvailableCarsByStationIdAsync(int stationId)
        {
            //  StationToCar
            return await general.StationToCars
                .Where(stationToCar => stationToCar.StationId == stationId && stationToCar.Car.Status==CarStatus.Available)
                .Select(stationToCar => stationToCar.Car)
                .ToListAsync();
        }

    }
}
