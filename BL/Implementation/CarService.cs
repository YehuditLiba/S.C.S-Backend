using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BL.DTO;
using Dal.DataObject;
using BL.Interfaces;
using Dal.DalImplements;
using Dal.Implemention;
using Dal.Interfaces;

namespace BL.Implementation
{
    public class CarService : ICarService
    {
        ICarRepository carRepository;
        IStationRepository _stationRepository;
        //auto mapper
        IMapper mapper;
        public CarService(ICarRepository carRepository, IStationRepository _stationRepository, IMapper mapper)
        {
            this.carRepository = carRepository;
            this._stationRepository = _stationRepository;
            this.mapper = mapper;
        }

        public async Task<int> CreateAsync(CarDTO item)
        {
            Car car = mapper.Map<Car>(item);
            return await carRepository.CreateAsync(car);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            return await carRepository.DeleteAsync(id);
        }
        public async Task<List<CarDTO>> ReadAllAsync()
        {
            List<Car> carsLst = await carRepository.ReadAllAsync();
            List<CarDTO> carsDtoLst = mapper.Map<List<CarDTO>>(carsLst);
            return carsDtoLst;
        }
        public async Task<CarDTO> ReadByIdAsync(int id)
        {
            Car car = await carRepository.ReadByIdAsync(id);
            CarDTO carDTO = mapper.Map<CarDTO>(car);
            return carDTO;
        }

        public async Task<bool> UpdateAsync(CarDTO newItem)
        {
            Car car = mapper.Map<Car>(newItem);
            return await carRepository.UpdateAsync(car);
        }

        public async Task<bool> ChangeTheCarModeAsync(int carId)
        {
            return await carRepository.ChangeTheCarModeAsync(carId);
        }
        public async Task<CarDTO> ReadByNameAsync(string carName)
        {
            Car car = await carRepository.GetByNameAsync(carName); // פנה למאגר הרכבים לפי שם
            CarDTO carDTO = mapper.Map<CarDTO>(car);
            return carDTO;
        }
        public async Task<bool> RentCarAsync(int carId, int stationId)
        {
            // 1. חיפוש הרכב והתחנה על פי המזהים
            var car = await carRepository.ReadByIdAsync(carId);
            var station = await _stationRepository.ReadByIdAsync(stationId);
            Console.WriteLine(station);
            Console.WriteLine(car);
            // 2. אם הרכב או התחנה לא קיימים או הרכב כבר הושכר
            if (car == null || station == null || car.Status == Dal.DataObject.CarStatus.Taken)
            {
                return false; // החזרת false אם לא ניתן להשכיר את הרכב
            }
            Console.WriteLine(car.Status);
            // 3. עדכון סטטוס הרכב ל-"Taken" (נלקח)
            car.Status = Dal.DataObject.CarStatus.Reserved;

            // 4. בדיקה אם כל הרכבים בתחנה אינם זמינים (תחנה מלאה)
            bool stationIsFull = station.StationToCars.All(stc => stc.CarId == null || carRepository.ReadByIdAsync(stc.CarId.GetValueOrDefault()).Result.Status != Dal.DataObject.CarStatus.Available);

            if (stationIsFull)
            {
                // 5. אם התחנה מלאה, עדכון הסטטוס שלה ל-"Full"
                station.IsFull = false;
            }

            var stationToCarEntry = new StationToCar
            {
                StationId = stationId,
                CarId = carId
            };

            // 7. שמירה במסד הנתונים
            bool carUpdated = await carRepository.UpdateAsync(car);
            bool stationUpdated = await _stationRepository.UpdateAsync(station);

            // 8. החזרת true אם העדכון בוצע בהצלחה עבור שני הגורמים
            return carUpdated && stationUpdated;
        }
        public async Task<bool> ReturnCarAsync(string carName, int stationId)
        {
            // 1. חיפוש הרכב לפי שם והתחנה לפי מזהה
            var car = await carRepository.GetByNameAsync(carName);
            var station = await _stationRepository.ReadByIdAsync(stationId);

            // 2. אם הרכב או התחנה לא נמצאו, או שהרכב אינו במצב "Taken"
            if (car != null || station != null || car.Status != Dal.DataObject.CarStatus.Taken||car.Status!=Dal.DataObject.CarStatus.Reserved)
            {
                return true;
            }

            // 3. עדכון סטטוס הרכב ל-"Available"
            car.Status = Dal.DataObject.CarStatus.Available;

            // 4. בדיקה אם התחנה מתמלאת עם החזרת הרכב
            bool stationIsFull = station.StationToCars.All(stc =>
                stc.CarId == null || carRepository.ReadByIdAsync(stc.CarId.GetValueOrDefault()).Result.Status == Dal.DataObject.CarStatus.Taken);

            if (stationIsFull)
            {
                station.IsFull = true;
            }

            // 5. שמירה במסד הנתונים
            bool carUpdated = await carRepository.UpdateAsync(car);
            bool stationUpdated = await _stationRepository.UpdateAsync(station);

            return carUpdated && stationUpdated;
        }

    }

}

