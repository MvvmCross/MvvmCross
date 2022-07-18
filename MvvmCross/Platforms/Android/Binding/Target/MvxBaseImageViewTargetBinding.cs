// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Graphics;
using Android.Widget;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Logging;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public abstract class MvxBaseImageViewTargetBinding
        : MvxAndroidTargetBinding
    {
        protected ImageView ImageView => (ImageView)Target;

        protected MvxBaseImageViewTargetBinding(ImageView imageView)
            : base(imageView)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected override void SetValueImpl(object target, object value)
        {
            var imageView = (ImageView)target;

            try
            {
                Bitmap bitmap;
                if (!GetBitmap(value, out bitmap))
                    return;
                SetImageBitmap(imageView, bitmap);
            }
            catch (Exception ex)
            {
                MvxLogHost.GetLog<MvxBaseImageViewTargetBinding>()?
                    .Log(LogLevel.Error, ex, "Failed to set bitmap on ImageView");
                throw;
            }
        }

        protected virtual void SetImageBitmap(ImageView imageView, Bitmap bitmap)
        {
            imageView.SetImageBitmap(bitmap);
        }

        protected abstract bool GetBitmap(object value, out Bitmap bitmap);
    }
}
