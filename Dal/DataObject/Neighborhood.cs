using System;
using System.Collections.Generic;

namespace Dal.DataObject;

public partial class Neighborhood
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CityId { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual ICollection<Street> Streets { get; } = new List<Street>();
}
