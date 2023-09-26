using System;
using System.Collections.Generic;

namespace PAEG1.Models;

public partial class Candidate
{
    public int Id { get; set; }

    public string? Surname { get; set; }

    public string? Name { get; set; }

    public string? Patronymic { get; set; }

    public string? Description { get; set; }

    public int? AmountOfVotes { get; set; }
}
