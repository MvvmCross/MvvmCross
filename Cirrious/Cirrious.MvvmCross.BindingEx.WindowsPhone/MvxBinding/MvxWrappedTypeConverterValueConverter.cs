// MvxWrappedTypeConverterValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.ComponentModel;
using Cirrious.CrossCore.Converters;

namespace Cirrious.MvvmCross.BindingEx.WindowsPhone.MvxBinding
{
    public class MvxWrappedTypeConverterValueConverter : IMvxValueConverter
    {
        private readonly TypeConverter _wrapped;

        public MvxWrappedTypeConverterValueConverter(TypeConverter wrapped)
        {
            _wrapped = wrapped;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return _wrapped.ConvertFrom(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            return _wrapped.ConvertTo(value, targetType);
        }
    }
}