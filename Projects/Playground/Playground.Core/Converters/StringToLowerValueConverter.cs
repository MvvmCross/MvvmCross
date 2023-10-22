using System.Globalization;
using MvvmCross.Converters;

namespace Playground.Core.Converters;

public sealed class StringToLowerValueConverter : MvxValueConverter<string, string>
{
    protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
    {
        return value.ToLower();
    }
}

public sealed class StringToUpperValueConverter : MvxValueConverter<string, string>
{
    protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
    {
        return value.ToUpper();
    }
}
