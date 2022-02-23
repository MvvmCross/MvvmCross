// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Windows.Input;
using MvvmCross.Converters;

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
