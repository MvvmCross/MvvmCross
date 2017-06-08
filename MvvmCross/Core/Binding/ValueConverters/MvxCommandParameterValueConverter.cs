// MvxCommandParameterValueConverter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;

namespace MvvmCross.Binding.ValueConverters
{
    using System.Windows.Input;

    using Platform.Converters;

    public class MvxCommandParameterValueConverter
        : MvxValueConverter<ICommand, ICommand>
    {
        protected override ICommand Convert(ICommand value, Type targetType, object parameter,
                                            CultureInfo culture)
        {
            return new MvxWrappingCommand(value, parameter);
        }
    }
}