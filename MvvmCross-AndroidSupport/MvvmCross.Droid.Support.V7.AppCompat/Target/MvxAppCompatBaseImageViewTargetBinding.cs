// MvxAppCompatBaseImageViewTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Droid.Support.V7.AppCompat.Target
{
    using System;

    using Android.Graphics;
    using Android.Support.V7.Widget;

    using MvvmCross.Binding;
    using MvvmCross.Binding.Droid.Target;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Platform;

    public abstract class MvxAppCompatBaseImageViewTargetBinding
        : MvxAndroidTargetBinding
    {
        protected AppCompatImageView ImageView => (AppCompatImageView)this.Target;

        protected MvxAppCompatBaseImageViewTargetBinding(AppCompatImageView imageView)
            : base(imageView)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected override void SetValueImpl(object target, object value)
        {
            var imageView = (AppCompatImageView)target;

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

        protected virtual void SetImageBitmap(AppCompatImageView imageView, Bitmap bitmap)
        {
            imageView.SetImageBitmap(bitmap);
        }

        protected abstract bool GetBitmap(object value, out Bitmap bitmap);
    }
}