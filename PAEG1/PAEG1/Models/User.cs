using System;
using System.Collections.Generic;

namespace PAEG1.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public int? ElectorateId { get; set; }

    public virtual Electorate? Electorate { get; set; }
}
