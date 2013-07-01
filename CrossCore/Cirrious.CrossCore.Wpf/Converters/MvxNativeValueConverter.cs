// MvxNativeValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;
using System.Windows.Data;
using Cirrious.CrossCore.Converters;

namespace Cirrious.CrossCore.Wpf.Converters
{
    public class MvxNativeValueConverter
        : IValueConverter
    {
        private readonly IMvxValueConverter _wrapped;

        public MvxNativeValueConverter(IMvxValueConverter wrapped)
        {
            _wrapped = wrapped;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _wrapped.Convert(value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _wrapped.ConvertBack(value, targetType, parameter, culture);
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
    }
}