using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dal.DataObject;
[Table("Neighborhoods")]
public partial class Neighborhood
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CityId { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual ICollection<Street> Streets { get; } = new List<Street>();
}
