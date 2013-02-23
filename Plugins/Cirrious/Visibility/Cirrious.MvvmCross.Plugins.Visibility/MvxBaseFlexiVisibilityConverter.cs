// MvxBaseFlexiVisibilityConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Plugins.Visibility
{
    public abstract class MvxBaseFlexiVisibilityConverter : MvxBaseVisibilityConverter
    {
        protected static bool IsATrueValue(object value, object parameter, bool defaultValue)
        {
            if (value == null)
            {
                return false;
            }

            if (value is bool)
            {
                return (bool) value;
            }

            if (value is int)
            {
                if (parameter == null)
                {
                    return (int) value > 0;
                }
                else
                {
                    return (int) value > int.Parse(parameter.ToString());
                }
            }

            if (value is double)
            {
                return (double) value > 0;
            }

            if (value is string)
            {
                return !string.IsNullOrWhiteSpace(value as string);
            }

            return defaultValue;
        }
    }
}