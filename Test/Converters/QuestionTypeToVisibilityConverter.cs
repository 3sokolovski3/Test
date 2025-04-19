using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Test.Enums;

namespace Test.Converters;

public class QuestionTypeToVisibilityConverter: IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int currentType && parameter is string expectedType)
        {
            if (Enum.TryParse(expectedType, out QuestionType expected))
            {
                return currentType == (int)expected;
            }
        }
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}