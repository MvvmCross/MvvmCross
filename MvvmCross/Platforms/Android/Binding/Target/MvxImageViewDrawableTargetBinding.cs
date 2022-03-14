// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Widget;
using MvvmCross.Binding;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public class MvxImageViewDrawableTargetBinding
        : MvxAndroidTargetBinding
    {
        protected ImageView ImageView => (ImageView)Target;

        public MvxImageViewDrawableTargetBinding(ImageView imageView)
            : base(imageView)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetValueType => typeof(int);

        protected override void SetValueImpl(object target, object value)
        {
            var imageView = (ImageView)target;

            if (!(value is int))
            {
                MvxBindingLog.Warning(
                    "Value was not a valid Drawable");
                imageView.SetImageDrawable(null);
                return;
            }

            var intValue = (int)value;

            if (intValue == 0)
                imageView.SetImageDrawable(null);
            else
                SetImage(imageView, intValue);
        }

        protected virtual void SetImage(ImageView imageView, int id)
        {
            var context = imageView.Context;
            Drawable drawable = context?.Resources?.GetDrawable(id, context.Theme);

            if (drawable != null)
                imageView.SetImageDrawable(drawable);
        }
    }
}
