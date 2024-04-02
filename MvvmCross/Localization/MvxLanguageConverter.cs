// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Globalization;
using MvvmCross.Converters;

namespace MvvmCross.Localization;

public class MvxLanguageConverter
    : MvxValueConverter
{
    public override object Convert(object value, Type? targetType, object? parameter, CultureInfo? culture)
    {
        if (value is not IMvxLanguageBinder binder)
            return MvxBindingConstant.UnsetValue;

        if (parameter == null)
            return MvxBindingConstant.UnsetValue;

        var translatedText = binder.GetText(parameter.ToString() ?? string.Empty);
        return translatedText ?? (object)MvxBindingConstant.UnsetValue;
    }
}
