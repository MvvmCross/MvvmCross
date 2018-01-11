using System;
using System.Collections.Generic;
using System.Globalization;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.Exceptions;

namespace MvvmCross.Binding.Test.Mocks
{
    public class MockValueConverter : IMvxValueConverter
    {
        public object ConversionResult { get; set; }
        public bool ThrowOnConversion { get; set; }

        public List<object> ConversionsRequested { get; } = new List<object>();
        public List<object> ConversionParameters { get; } = new List<object>();
        public List<Type> ConversionTypes { get; } = new List<Type>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ConversionsRequested.Add(value);
            ConversionParameters.Add(parameter);
            ConversionTypes.Add(targetType);
            if (ThrowOnConversion)
                throw new MvxException("Conversion throw requested");
            return ConversionResult;
        }

        public object ConversionBackResult { get; set; }
        public bool ThrowOnConversionBack { get; set; }
        public List<object> ConversionsBackRequested { get; } = new List<object>();
        public List<object> ConversionBackParameters { get; } = new List<object>();
        public List<Type> ConversionBackTypes { get; } = new List<Type>();

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ConversionsBackRequested.Add(value);
            ConversionBackParameters.Add(parameter);
            ConversionBackTypes.Add(targetType);
            if (ThrowOnConversionBack)
                throw new MvxException("Conversion throw requested");
            return ConversionBackResult;
        }
    }
}