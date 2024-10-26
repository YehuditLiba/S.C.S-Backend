using Dal.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static GoogleMaps.LocationServices.Directions;
using System.Text.Json.Serialization;

namespace BL.DTO
{
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public enum CarStatus
    {
        Available = 0,
        Taken = 1,
        Returned = 2,
        Reserved = 3
    }
    public class CarDTO
    {
        public string Name { get; set; }

        public int LicensePlate { get; set; }

        public int NumOfSeets { get; set; }
        public CarStatus Status { get; set; }

        public string StatusAsString => Status.ToString();
        public CarDTO(string name, int LicensePlate, int NumOfSeets, CarStatus status)
        {
            this.Name = name;
            this.LicensePlate = LicensePlate;
            this.NumOfSeets = NumOfSeets;
            this.Status = status;

        }
    }
}
