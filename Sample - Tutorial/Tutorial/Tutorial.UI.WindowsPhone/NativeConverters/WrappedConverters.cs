using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Cirrious.MvvmCross.Converters;
using Cirrious.MvvmCross.Converters.Visibility;
using Cirrious.MvvmCross.WindowsPhone.Platform.Converters;

namespace Tutorial.UI.WindowsPhone.NativeConverters
{
    public class VisibilityConverter : MvxNativeValueConverter<MvxVisibilityConverter>
    {
    }

    public class TypeToNameConverter : MvxNativeValueConverter<Converters.TypeToNameConverter>
    {
    }

    public class StringReverseValueConverter : MvxNativeValueConverter<Core.Converters.StringReverseValueConverter>
    {
    }

    public class StringLengthValueConverter : MvxNativeValueConverter<Core.Converters.StringLengthValueConverter>
    {
    }

    public class FloatConverter : MvxNativeValueConverter<Core.Converters.FloatConverter>
    {
    }

    public class IntConverter : MvxNativeValueConverter<Core.Converters.IntConverter>
    {
    }
}
