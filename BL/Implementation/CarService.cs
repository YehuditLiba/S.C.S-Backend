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
            var car = await carRepository.ReadByIdAsync(carId);
            var station = await _stationRepository.ReadByIdAsync(stationId);
            Console.WriteLine(station);
            Console.WriteLine(car);

            if (car == null || station == null || car.Status == Dal.DataObject.CarStatus.Taken)
            {
                return false; // false 
            }
            Console.WriteLine(car.Status);
            // Taken
            car.Status = Dal.DataObject.CarStatus.Reserved;

            
            bool stationIsFull = station.StationToCars.All(stc => stc.CarId == null || carRepository.ReadByIdAsync(stc.CarId.GetValueOrDefault()).Result.Status != Dal.DataObject.CarStatus.Available);

            if (stationIsFull)
            {
                station.IsFull = false;
            }

            var stationToCarEntry = new StationToCar
            {
                StationId = stationId,
                CarId = carId
            };

            bool carUpdated = await carRepository.UpdateAsync(car);
            bool stationUpdated = await _stationRepository.UpdateAsync(station);

            return carUpdated && stationUpdated;
        }
        public async Task<bool> ReturnCarAsync(string carName, int stationId)
        {
            var car = await carRepository.GetByNameAsync(carName);
            var station = await _stationRepository.ReadByIdAsync(stationId);

            if (car == null || station == null || (car.Status != Dal.DataObject.CarStatus.Taken && car.Status != Dal.DataObject.CarStatus.Reserved))
            {
                return false;
            }

            //"Available"
            car.Status = Dal.DataObject.CarStatus.Available;

            //  StationToCar 
            var stationToCarEntry = station.StationToCars.FirstOrDefault(stc => stc.CarId == null);
            if (stationToCarEntry != null)
            {
                stationToCarEntry.CarId = car.Id;
            }
            else
            {
                //  StationToCar
                station.StationToCars.Add(new StationToCar
                {
                    StationId = stationId,
                    CarId = car.Id
                });
            }

            bool stationIsFull = station.StationToCars.All(stc =>
                stc.CarId != null && carRepository.ReadByIdAsync(stc.CarId.GetValueOrDefault()).Result.Status == Dal.DataObject.CarStatus.Available);

            if (stationIsFull)
            {
                station.IsFull = true;
            }

            bool carUpdated = await carRepository.UpdateAsync(car);
            bool stationUpdated = await _stationRepository.UpdateAsync(station);

            return carUpdated && stationUpdated;
        }
    }
  }

