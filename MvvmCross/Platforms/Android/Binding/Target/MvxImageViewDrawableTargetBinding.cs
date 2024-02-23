// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using Android.Graphics.Drawables;
using MvvmCross.Binding;

namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxImageViewDrawableTargetBinding(ImageView imageView)
    : MvxAndroidTargetBinding(imageView)
{
    protected ImageView? ImageView => (ImageView?)Target;

    public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

    public override Type TargetValueType => typeof(int);

    protected override void SetValueImpl(object target, object? value)
    {
        var view = (ImageView)target;

        if (value is not int resourceIdentifier)
        {
            MvxBindingLog.Warning(
                "Value was not a valid Drawable");
            imageView.SetImageDrawable(null);
            return;
        }

        if (resourceIdentifier == 0)
            view.SetImageDrawable(null);
        else
            SetImage(view, resourceIdentifier);
    }

    protected virtual void SetImage(ImageView view, int id)
    {
        var context = view.Context;
        Drawable? drawable = context?.Resources?.GetDrawable(id, context.Theme);
        if (drawable != null)
            view.SetImageDrawable(drawable);
    }
}
