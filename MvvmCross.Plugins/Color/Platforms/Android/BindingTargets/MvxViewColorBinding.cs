// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Views;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.Binding.Target;

namespace MvvmCross.Plugin.Color.Platforms.Android.BindingTargets
{
    [Preserve(AllMembers = true)]
    public abstract class MvxViewColorBinding
        : MvxAndroidTargetBinding
    {
        protected View TextView => (View)Target;

        protected MvxViewColorBinding(View view)
            : base(view)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetValueType => typeof(global::Android.Graphics.Color);
    }
}
