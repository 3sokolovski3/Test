using Test.Enums;

namespace Test.Models;

public class QuestionTypeItem
{
    public QuestionType Type { get; }
    public string DisplayName { get; }

    public QuestionTypeItem(QuestionType type, string displayName)
    {
        Type = type;
        DisplayName = displayName;
    }
}