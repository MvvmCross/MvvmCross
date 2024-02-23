// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System.Reflection;

namespace MvvmCross.Platforms.Ios.Binding.Target;

public class MvxUIDatePickerCountDownDurationTargetBinding(UIDatePicker target, PropertyInfo targetPropertyInfo)
    : MvxBaseUIDatePickerTargetBinding(target, targetPropertyInfo)
{
    protected override object GetValueFrom(UIDatePicker view)
    {
        return view.CountDownDuration;
    }

    public override Type TargetValueType => typeof(double);
}
