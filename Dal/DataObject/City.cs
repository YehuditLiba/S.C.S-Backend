using System;
using System.Collections.Generic;

namespace Dal.DataObject;

public partial class City
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

   
    public virtual ICollection<Neighborhood> Neighborhoods { get; } = new List<Neighborhood>();
    public virtual ICollection<Rentals> Rentals { get; set; }
}
