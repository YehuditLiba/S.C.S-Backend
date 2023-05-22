using Dal.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTO
{
    public class StationDTO
    {
        public int Number { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Neighborhood { get; set; }
        public StationDTO(int num, string street, string neighborhood, string city)
        {
            this.Number = num;
            this.Neighborhood = neighborhood;
            this.Street = street;
            this.City = city;
        }
    }
}
