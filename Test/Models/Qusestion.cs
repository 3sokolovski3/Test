using System;
using System.Collections.Generic;

namespace Test.Models;

public partial class Qusestion
{
    public int Id { get; set; }

    public string? Text { get; set; }

    public int? Type { get; set; }

    public int? TestId { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual Test? Test { get; set; }
}
