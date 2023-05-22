using AutoMapper;
using BL.DTO;
using BL.Interfaces;
using Dal.DataObject;
using Dal.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Implementation
{
    public class StationService : IStationService
    {
        IStationRepository stationRepository;
        IMapper mapper;
        public StationService(IStationRepository stationRepository, IMapper mapper)
        {
            this.stationRepository = stationRepository;
            this.mapper = mapper;
        }

        public Task<Station> GetNearestStation(StationDTO stationDTO) 
        {
            //find the nearset station and convert it from stationDTO to Station

            //Station station = mapper.Map<Station>(stationDTO);
            //return await stationRepository.GetNearestStation(station);

            throw new NotImplementedException();
        }
    }
}
