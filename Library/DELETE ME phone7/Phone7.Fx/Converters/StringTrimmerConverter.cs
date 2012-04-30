using System;
using System.Globalization;
using System.Windows.Data;

namespace Phone7.Fx.Converters
{
    public sealed class StringTrimmerConverter : IValueConverter
    {

        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The <see cref="T:System.Type"/> of data expected by the target dependency property.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="culture">The culture of the conversion.</param>
        /// <returns>
        /// The value to be passed to the target dependency property.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string))
            {
                throw new ArgumentException( "Target type must be a string !", "targetType");
            }

            string  str = value as string;
            if (String.IsNullOrEmpty(str))
                return string.Empty;

            if (parameter != null)
            {
                int maxLength;
                if (int.TryParse(parameter.ToString(), out maxLength) && (str.Length > maxLength))
                    return string.Format("{0}...", str.Substring(0, maxLength));
            }
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}