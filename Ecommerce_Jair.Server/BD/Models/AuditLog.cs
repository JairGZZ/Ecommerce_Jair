using System;
using System.Collections.Generic;

namespace Ecommerce_Jair.Server.BD.Models;

public partial class AuditLog
{
    public int AuditId { get; set; }

    public int? UserId { get; set; }

    public string Action { get; set; } = null!;

    public string TableName { get; set; } = null!;

    public int? RecordId { get; set; }

    public string? OldData { get; set; }

    public string? NewData { get; set; }

    public DateTime? ChangedAt { get; set; }

    public string? Ipaddress { get; set; }

    public string? BrowserInfo { get; set; }
}
