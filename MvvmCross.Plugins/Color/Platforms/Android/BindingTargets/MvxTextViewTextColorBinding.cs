// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Widget;

namespace MvvmCross.Plugin.Color.Platforms.Android.BindingTargets
{
    [Preserve(AllMembers = true)]
    public class MvxTextViewTextColorBinding
        : MvxViewColorBinding
    {
        public MvxTextViewTextColorBinding(TextView textView)
            : base(textView)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            var textView = (TextView)target;
            textView?.SetTextColor((global::Android.Graphics.Color)value);
        }
    }
}
