using System.Drawing;
using System.Globalization;
using MvvmCross.Plugin.Color;

namespace Playground.Core.Converters;

// Sample converter to show issue found in GH issue #4803
public sealed class TextToColorValueConverter : MvxColorValueConverter
{
    protected override Color Convert(object value, object parameter, CultureInfo culture)
    {
        if (value is not string stringValue)
            return Color.Magenta;

        return stringValue switch
        {
            "I am green!" => Color.Green,
            "I am yellow!" => Color.Yellow,
            "I am brown!" => Color.Brown,
            "I am orange!" => Color.Orange,
            _ => Color.Black
        };
    }
}
