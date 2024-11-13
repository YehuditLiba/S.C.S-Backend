using BL.DTO;
using Dal.DataObject;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Interfaces
{
    public interface IStationService : IService<StationDTO>
    {
        // פונקציה לחיפוש תחנה קרובה לפי קואורדינטות (x, y)
        Task<StationDTO> GetNearestStation(double x, double y);
        Task<List<Car>> GetAvailableCarsInFullStationAsync(int stationId);
        Task<StationDTO> ReadByIdWithCarsAsync(int stationId);
        Task<StationDTO> GetLucrativeStation(int numberOfRentalDays,double x,double y );

    }
}
