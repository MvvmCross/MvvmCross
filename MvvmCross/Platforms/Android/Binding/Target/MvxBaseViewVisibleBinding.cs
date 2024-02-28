// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using Android.Views;

namespace MvvmCross.Platforms.Android.Binding.Target;

public abstract class MvxBaseViewVisibleBinding(object target)
    : MvxAndroidTargetBinding(target)
{
    protected View? View => (View?)Target;

    public override Type TargetValueType => typeof(bool);
}
