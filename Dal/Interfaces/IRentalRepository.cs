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
       // Task<int> CreateAsync(Rentals rental);
      //  Task<List<Rentals>> GetRentalsByCarIdAsync(int carId);
    }
}
