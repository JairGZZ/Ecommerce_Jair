﻿using System;
using System.Collections.Generic;

namespace Ecommerce_Jair.Server.Models;

public partial class UserRole
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public bool? IsPrimary { get; set; }

    public DateTime? AssignedDate { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
