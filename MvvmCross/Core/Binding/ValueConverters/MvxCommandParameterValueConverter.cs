// MvxCommandParameterValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Converters;
using System.Windows.Input;

namespace Cirrious.MvvmCross.Binding.ValueConverters
{
    public class MvxCommandParameterValueConverter
        : MvxValueConverter<ICommand, ICommand>
    {
        protected override ICommand Convert(ICommand value, System.Type targetType, object parameter,
                                            System.Globalization.CultureInfo culture)
        {
            return new MvxWrappingCommand(value, parameter);
        }
    }
}