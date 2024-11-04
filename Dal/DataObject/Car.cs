using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Dal.DataObject;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CarStatus
{
    Available = 0,
    Taken = 1,
    Returned = 2,
    Reserved = 3
}
public partial class Car
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int LicensePlate { get; set; }

    public int NumOfSeets { get; set; }

    //public bool IsAvailable { get; set; }
    //IsEvalable from enum 
    public CarStatus Status { get; set; }

    public virtual StationToCar? StationToCar { get; set; }
    public virtual ICollection<Rentals> Rentals { get; set; }
}
