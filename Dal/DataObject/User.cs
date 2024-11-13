﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dal.DataObject;
[Table("Users")]
public partial class User
{
    public int Code { get; set; }

    public bool? IsManager { get; set; }

    public string Name { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;
     public virtual ICollection<Rentals> Rentals { get; set; }
}
