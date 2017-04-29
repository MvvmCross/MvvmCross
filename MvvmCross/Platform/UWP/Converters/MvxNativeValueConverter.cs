// MvxNativeValueConverter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using MvvmCross.Platform.Converters;

namespace MvvmCross.Platform.Uwp.Converters
{
    public class MvxNativeValueConverter
        : IValueConverter
    {
        public MvxNativeValueConverter(IMvxValueConverter wrapped)
        {
            Wrapped = wrapped;
        }

        protected IMvxValueConverter Wrapped { get; }

        public virtual object Convert(object value, Type targetType, object parameter, string language)
        {
            // note - Language ignored here!
            var toReturn = Wrapped.Convert(value, targetType, parameter, CultureInfo.CurrentUICulture);
            return MapIfSpecialValue(toReturn);
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            // note - Language ignored here!
            var toReturn = Wrapped.ConvertBack(value, targetType, parameter, CultureInfo.CurrentUICulture);
            return MapIfSpecialValue(toReturn);
        }

        private static object MapIfSpecialValue(object toReturn)
        {
            if (toReturn == MvxBindingConstant.DoNothing)
            {
                Mvx.Trace("DoNothing does not have an equivalent in WinRT - returning UnsetValue instead");
                return DependencyProperty.UnsetValue;
            }

            if (toReturn == MvxBindingConstant.UnsetValue)
                return DependencyProperty.UnsetValue;

            return toReturn;
        }
    }

    public class MvxNativeValueConverter<T>
        : MvxNativeValueConverter
        where T : IMvxValueConverter, new()
    {
        public MvxNativeValueConverter()
            : base(new T())
        {
        }

        protected new T Wrapped => (T) base.Wrapped;
    }
}