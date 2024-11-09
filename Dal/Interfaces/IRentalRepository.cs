using Dal.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Interfaces
{
    public interface IRentalRepository:IRepository<Rentals>
    {
        Task<Station> GetStationByCarIdAsync(int carId);      //  Task<List<Rentals>> GetRentalsByCarIdAsync(int carId);
    }
}
