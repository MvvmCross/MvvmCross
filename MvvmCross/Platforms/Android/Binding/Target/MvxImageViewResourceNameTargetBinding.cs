// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using MvvmCross.Binding;

namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxImageViewResourceNameTargetBinding(ImageView imageView)
    : MvxImageViewDrawableTargetBinding(imageView)
{
    public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

    public override Type TargetValueType => typeof(string);

    protected override void SetImage(ImageView view, int id)
    {
        view.SetImageResource(id);
    }
}
