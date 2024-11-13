using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dal.DataObject;
[Table("Cities")]
public partial class City
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

   
    public virtual ICollection<Neighborhood> Neighborhoods { get; } = new List<Neighborhood>();
}
