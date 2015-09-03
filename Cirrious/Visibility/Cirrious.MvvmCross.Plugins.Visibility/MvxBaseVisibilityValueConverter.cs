// MvxBaseVisibilityValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections;
using System.Globalization;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore;
using Cirrious.CrossCore.UI;

namespace Cirrious.MvvmCross.Plugins.Visibility
{
	public abstract class MvxBaseVisibilityValueConverter<T>
		: MvxBaseVisibilityValueConverter
	{
		protected sealed override MvxVisibility Convert(object value, object parameter, CultureInfo culture)
		{
			return Convert((T)value, parameter, culture);
		}

		protected abstract MvxVisibility Convert(T value, object parameter, CultureInfo culture);
	}

    public abstract class MvxBaseVisibilityValueConverter
        : MvxValueConverter          
    {
        private IMvxNativeVisibility _nativeVisiblity;

        private IMvxNativeVisibility NativeVisibility
        {
            get
            {
                if (_nativeVisiblity == null)
                {
                    _nativeVisiblity = Mvx.Resolve<IMvxNativeVisibility>();
                }

                return _nativeVisiblity;
            }
        }

		protected abstract MvxVisibility Convert(object value, object parameter, CultureInfo culture);

        public override sealed object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mvx = Convert(value, parameter, culture);
            return NativeVisibility.ToNative(mvx);
        }

        public override sealed object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return base.ConvertBack(value, targetType, parameter, culture);
        }

        protected virtual bool IsATrueValue(object value, object parameter, bool defaultValue)
        {
            if (value == null)
            {
                return false;
            }

            if (value is bool)
            {
                return (bool)value;
            }

            if (value is int)
            {
                if (parameter == null)
                {
                    return (int)value > 0;
                }
                else
                {
                    return (int)value > int.Parse(parameter.ToString());
                }
            }

            if (value is double)
            {
                return (double)value > 0;
            }

            if (value is string)
            {
                return !string.IsNullOrWhiteSpace(value as string);
            }

            // 19/Mar/2013 - decided *not* to test IEnumerable - if user's need this then they will have to provide overrides
            //if (value is IEnumerable)
            //{
            //    var enumerable = value as IEnumerable;
            //    return enumerable.GetEnumerator().MoveNext();
            //}

            return defaultValue;
        }
    }
}