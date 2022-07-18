// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Views;

namespace MvvmCross.Plugin.Color.Platforms.Android.BindingTargets
{
    [Preserve(AllMembers = true)]
    public class MvxViewBackgroundColorBinding
        : MvxViewColorBinding
    {
        public MvxViewBackgroundColorBinding(View view)
            : base(view)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = (View)target;
            view?.SetBackgroundColor((global::Android.Graphics.Color)value);
        }
    }
}
