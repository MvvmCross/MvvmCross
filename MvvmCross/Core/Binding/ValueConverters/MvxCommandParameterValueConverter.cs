// MvxCommandParameterValueConverter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;
using System.Windows.Input;
using MvvmCross.Platform.Converters;

namespace MvvmCross.Binding.ValueConverters
{
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