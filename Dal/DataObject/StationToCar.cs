using System;
using System.Collections.Generic;

namespace Dal.DataObject;

public partial class StationToCar
{
    public int Id { get; set; }

    public int StationId { get; set; }

    public int? CarId { get; set; }

    public virtual Car? Car { get; set; }

    public virtual Station Station { get; set; } = null!;
}
