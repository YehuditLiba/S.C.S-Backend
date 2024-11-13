using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dal.DataObject;
[Table("Streets")]
public partial class Street
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int NeigborhoodId { get; set; }

    public virtual Neighborhood Neigborhood { get; set; } = null!;

    public virtual ICollection<Station> Stations { get; } = new List<Station>();
}
