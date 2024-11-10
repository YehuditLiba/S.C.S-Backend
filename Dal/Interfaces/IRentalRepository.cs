using Dal.DataObject;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Interfaces
{
    public interface IRentalRepository:IRepository<Rentals>
    {
        Task<Station> GetStationByCarIdAsync(int carId);
        Task<List<Rentals>> GetRentalsByUserNameAsync(string name);
    }
}
