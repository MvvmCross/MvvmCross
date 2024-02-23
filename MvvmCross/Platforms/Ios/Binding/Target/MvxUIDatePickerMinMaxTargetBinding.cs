// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System.Reflection;

namespace MvvmCross.Platforms.Ios.Binding.Target;

public class MvxUIDatePickerMinMaxTargetBinding
    : MvxBaseUIDatePickerTargetBinding
{
    public MvxUIDatePickerMinMaxTargetBinding(UIDatePicker target, PropertyInfo targetPropertyInfo)
        : base(target, targetPropertyInfo)
    {
        var targetPropertyName = targetPropertyInfo.Name;
        if (targetPropertyName == nameof(UIDatePicker.Date))
            throw new ArgumentException("This binding cannot be used with the Date property as the target.");
    }

    protected override object GetValueFrom(UIDatePicker view)
    {
        // This method should never be called.
        throw new NotImplementedException();
    }

    protected override object MakeSafeValue(object? value)
    {
        if (value == null)
            return NSDate.FromTimeIntervalSince1970(0);
        
        var valueUtc = ToUtcTime((DateTime)value);
        var valueNSDate = valueUtc.ToNSDate();

        return valueNSDate;
    }
}
