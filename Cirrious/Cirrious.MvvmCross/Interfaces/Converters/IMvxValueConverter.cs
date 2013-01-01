// IMvxValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;

namespace Cirrious.MvvmCross.Interfaces.Converters
{
#if WINDOWS_PHONE
    public interface IMvxValueConverter : System.Windows.Data.IValueConverter
    {        
    }
#elif NETFX_CORE
    public interface IMvxValueConverter : Windows.UI.Xaml.Data.IValueConverter
    {        
    }    
#else
    public interface IMvxValueConverter
    {
        object Convert(object value, Type targetType, object parameter, CultureInfo culture);
        object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
#endif
}