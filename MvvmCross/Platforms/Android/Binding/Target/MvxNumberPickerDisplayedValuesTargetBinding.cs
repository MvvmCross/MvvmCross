// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;

namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxNumberPickerDisplayedValuesTargetBinding(NumberPicker target)
    : MvxTargetBinding<NumberPicker, IEnumerable<string>?>(target)
{
    public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

    protected override void SetValue(IEnumerable<string>? value)
    {
        if (Target == null)
            return;

        var arrayVal = value?.ToArray() ?? [];

        if (Target.MaxValue == 0)
            Target.MaxValue = arrayVal.Length - 1;
        Target.SetDisplayedValues(arrayVal);
        Target.Invalidate();
    }
}
