using System;
using System.Collections.Generic;

namespace Test.Models;

public partial class Answer
{
    public int Id { get; set; }

    public int? QuestionId { get; set; }

    public string? Text { get; set; }

    public int? IsCorrect { get; set; }

    public virtual Qusestion? Question { get; set; }
}
