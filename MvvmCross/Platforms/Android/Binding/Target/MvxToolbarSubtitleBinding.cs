// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public class MvxToolbarSubtitleBinding
        : MvxConvertingTargetBinding
    {
        public MvxToolbarSubtitleBinding(Toolbar toolbar)
            : base(toolbar)
        {
        }

        public override Type TargetValueType => typeof(string);
        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected override void SetValueImpl(object target, object value)
        {
            if (target is Toolbar toolbar)
                toolbar.Subtitle = (string)value;
        }

        protected Toolbar Toolbar => (Toolbar)Target;
    }
}
