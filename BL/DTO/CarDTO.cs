using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTO
{
    public class CarDTO
    {
        public string Name { get; set; }

        public int LicensePlate { get; set; }

        public int NumOfSeets { get; set; }

        public bool IsAvailable { get; set; }
        public CarDTO(string name, int LicensePlate, int NumOfSeets, bool IsAvailable)
        {
            this.Name = name;
            this.LicensePlate = LicensePlate;
            this.NumOfSeets = NumOfSeets;
            this.IsAvailable = IsAvailable;

        }
    }
}
