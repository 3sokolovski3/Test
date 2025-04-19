using System;
using System.Collections.Generic;

namespace Test.Models;

public partial class Test
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Qusestion> Qusestions { get; set; } = new List<Qusestion>();
}
