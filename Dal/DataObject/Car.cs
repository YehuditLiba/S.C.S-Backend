using System;
using System.Collections.Generic;

namespace Dal.DataObject;

public partial class Car
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int LicensePlate { get; set; }

    public int NumOfSeets { get; set; }

    public bool IsAvailable { get; set; }

    public virtual StationToCar? StationToCar { get; set; }
}
