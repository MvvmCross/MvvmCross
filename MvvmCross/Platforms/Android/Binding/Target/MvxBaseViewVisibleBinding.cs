// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Views;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public abstract class MvxBaseViewVisibleBinding
        : MvxAndroidTargetBinding
    {
        protected View View => (View)Target;

        protected MvxBaseViewVisibleBinding(object target)
            : base(target)
        {
        }

        public override Type TargetValueType => typeof(bool);
    }
}
