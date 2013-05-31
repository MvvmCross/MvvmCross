// MvxNativeValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;
using Cirrious.CrossCore.Converters;
using Windows.UI.Xaml.Data;

namespace Cirrious.CrossCore.WindowsStore.Converters
{
    public class MvxNativeValueConverter
        : IValueConverter
    {
        private readonly IMvxValueConverter _wrapped;

        public MvxNativeValueConverter(IMvxValueConverter wrapped)
        {
            _wrapped = wrapped;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // note - Language ignored here!
            return _wrapped.Convert(value, targetType, parameter, CultureInfo.CurrentUICulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            // note - Language ignored here!
            return _wrapped.ConvertBack(value, targetType, parameter, CultureInfo.CurrentUICulture);
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