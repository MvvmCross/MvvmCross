// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;

namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxImageViewDrawableNameTargetBinding(ImageView imageView)
    : MvxImageViewDrawableTargetBinding(imageView)
{
    public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

    public override Type TargetValueType => typeof(string);

    protected override void SetValueImpl(object target, object? value)
    {
        var view = (ImageView)target;

        if (value is not string drawableName)
        {
            MvxBindingLog.Instance?.LogWarning(
                "Value '{Value}' could not be parsed as a valid string identifier", value);
            view.SetImageDrawable(null);
            return;
        }

        var appContext = Application.Context;
        var resources = appContext.Resources;
        if (resources == null)
            return;

        var id = resources.GetIdentifier(drawableName, "drawable", appContext.PackageName);
        if (id == 0)
        {
            MvxBindingLog.Instance?.LogWarning(
                "Value '{DrawableName}' was not a known drawable name", drawableName);
            view.SetImageDrawable(null);
            return;
        }

        base.SetValueImpl(target, id);
    }
}
