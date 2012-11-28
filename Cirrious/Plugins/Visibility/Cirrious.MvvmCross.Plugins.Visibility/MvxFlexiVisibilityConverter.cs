#region Copyright
// <copyright file="MvxFlexiVisibilityConverter.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Globalization;

namespace Cirrious.MvvmCross.Plugins.Visibility
{
    public class MvxFlexiVisibilityConverter : MvxBaseFlexiVisibilityConverter
    {
        public static bool IsATrueValue(object value, object parameter, bool defaultValue)
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
                return string.IsNullOrWhiteSpace(value as string);
            }

            return defaultValue;
        }

        public override MvxVisibility ConvertToMvxVisibility(object value, object parameter, CultureInfo culture)
        {
            var visibility = IsATrueValue(value, parameter, true);
            return visibility ? MvxVisibility.Visible : MvxVisibility.Collapsed;
        }
    }
}