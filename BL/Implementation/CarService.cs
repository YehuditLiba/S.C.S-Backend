using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BL.DTO;
using BL.Interfaces;
using Dal.DataObject;
using Dal.Interfaces;

namespace BL.Implementation
{
    public class CarService : ICarService
    {
        ICarRepository carRepository;
        //auto mapper
        IMapper mapper;
        public CarService(ICarRepository carRepository, IMapper mapper)
        {
            this.carRepository = carRepository;
            this.mapper = mapper;
        }

        public async Task<int> CreateAsync(CarDTO item)
        {
            Car car = mapper.Map<Car>(item);
            return await carRepository.CreateAsync(car);
        }
        public async Task<bool> DeleteAsync(int id)
        {
              return  await carRepository.DeleteAsync(id);
        }

        public Task<List<CarDTO>> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CarDTO> ReadByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(CarDTO newItem)
        {
            throw new NotImplementedException();
        }
    }
}
