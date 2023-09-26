using System;
using System.Collections.Generic;

namespace PAEG1.Models;

public partial class Electorate
{
    public int Id { get; set; }

    public string? Surname { get; set; }

    public string? Name { get; set; }

    public string? Patronymic { get; set; }

    public int? HasVoted { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
