// MvxBaseImageViewTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Target
{
    using System;

    using Android.Graphics;
    using Android.Widget;

    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Platform;

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
                if (!this.GetBitmap(value, out bitmap))
                    return;
                this.SetImageBitmap(imageView, bitmap);
            }
            catch (Exception ex)
            {
                MvxTrace.Error(ex.ToLongString());
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