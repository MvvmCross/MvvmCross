// MvxNativeValueConverter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using MvvmCross.Platform.Converters;

namespace MvvmCross.Platform.Wpf.Converters
{
    public class MvxNativeValueConverter
        : MarkupExtension, IValueConverter
    {
        private readonly IMvxValueConverter _wrapped;

        public MvxNativeValueConverter(IMvxValueConverter wrapped)
        {
            _wrapped = wrapped;
        }

        protected IMvxValueConverter Wrapped => _wrapped;

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var toReturn = _wrapped.Convert(value, targetType, parameter, culture);
            return MapIfSpecialValue(toReturn);
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var toReturn = _wrapped.ConvertBack(value, targetType, parameter, culture);
            return MapIfSpecialValue(toReturn);
        }

        private static object MapIfSpecialValue(object toReturn)
        {
            if (toReturn == MvxBindingConstant.DoNothing)
            {
                return Binding.DoNothing;
            }

            if (toReturn == MvxBindingConstant.UnsetValue)
            {
                return DependencyProperty.UnsetValue;
            }

            return toReturn;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) 
        { 
            return this; 
        }
    }

    public class MvxNativeValueConverter<T>
        : MvxNativeValueConverter
        where T : IMvxValueConverter, new()
    {
        protected new T Wrapped => (T)base.Wrapped;

        public MvxNativeValueConverter()
            : base(new T())
        {
        }
    }
}