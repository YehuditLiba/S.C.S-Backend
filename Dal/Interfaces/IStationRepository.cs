using Dal.DataObject;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal.Interfaces
{
    public interface IStationRepository : IRepository<Station>
    {
        // שיטה לחיפוש התחנה הקרובה ביותר על פי קואורדינטות X ו-Y
        Task<Station> GetNearestStation(double x, double y);
        Task<bool> IsStationFullAsync(int stationId);
        Task<Station> ReadByIdWithCarsAsync(int stationId);
        Task<Station> ReadByNumberAsync(int stationNumber);


    }
}
