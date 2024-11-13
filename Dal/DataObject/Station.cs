using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dal.DataObject;
[Table("Stations")]
public partial class Station
{
    public int Id { get; set; }

    public double X { get; set; }

    public double Y { get; set; }

    public int? Number { get; set; }

    public int StreetId { get; set; }

    public bool? IsCenteral { get; set; }
    public bool? IsFull { get; set; }

    public virtual ICollection<StationToCar> StationToCars { get; } = new List<StationToCar>();
    public virtual Street Street { get; set; } = null!;

}
