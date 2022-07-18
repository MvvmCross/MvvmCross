// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Widget;
using MvvmCross.Binding;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public class MvxImageViewDrawableNameTargetBinding
        : MvxImageViewDrawableTargetBinding
    {
        public MvxImageViewDrawableNameTargetBinding(ImageView imageView)
            : base(imageView)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetValueType => typeof(string);

        protected override void SetValueImpl(object target, object value)
        {
            var imageView = (ImageView)target;

            if (!(value is string))
            {
                MvxBindingLog.Warning(
                    "Value '{0}' could not be parsed as a valid string identifier", value);
                imageView.SetImageDrawable(null);
                return;
            }

            var resources = AndroidGlobals.ApplicationContext.Resources;
            var id = resources.GetIdentifier((string)value, "drawable", AndroidGlobals.ApplicationContext.PackageName);
            if (id == 0)
            {
                MvxBindingLog.Warning(
                    "Value '{0}' was not a known drawable name", value);
                imageView.SetImageDrawable(null);
                return;
            }

            base.SetValueImpl(target, id);
        }
    }
}
