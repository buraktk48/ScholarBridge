using System;
using System.Collections.Generic;

namespace ScholarBridge.Models;

public partial class UserRole
{
    public int UserRoleId { get; set; }

    public int? UserFkId { get; set; }

    public int? RoleFkId { get; set; }

    public virtual Role? RoleFk { get; set; }

    public virtual User? UserFk { get; set; }
}
