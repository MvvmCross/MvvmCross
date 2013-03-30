using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Cirrious.Conference.Core.Converters;
using Cirrious.CrossCore.WindowsPhone.Converters;
using Cirrious.MvvmCross.Localization;
using Cirrious.MvvmCross.Plugins.Visibility;

namespace Cirrious.Conference.UI.WP7.NativeConverters
{
    public class LanguageBinderConverter : MvxNativeValueConverter<MvxLanguageConverter>
    {        
    }

    public class VisibilityConverter : MvxNativeValueConverter<MvxVisibilityValueConverter>
    {
    }

    public class SimpleDateConverter : MvxNativeValueConverter<SimpleDateValueConverter>
    {
    }

    public class SessionSmallDetailsConverter : MvxNativeValueConverter<SessionSmallDetailsValueConverter>
    {
    }

    public class SponsorImageConverter : MvxNativeValueConverter<SponsorImageValueConverter>
    {
    }

    public class TimeAgoConverter : MvxNativeValueConverter<TimeAgoValueConverter>
    {
    }
}
