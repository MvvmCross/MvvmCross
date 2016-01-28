// MvxAppCompatBaseStreamImageViewTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Droid.Support.V7.AppCompat.Target
{
    using System.IO;

    using Android.Content.Res;
    using Android.Graphics;
    using Android.Graphics.Drawables;
    using Android.Support.V7.Widget;

    public abstract class MvxAppCompatBaseStreamImageViewTargetBinding
        : MvxAppCompatBaseImageViewTargetBinding
    {
        protected MvxAppCompatBaseStreamImageViewTargetBinding(AppCompatImageView imageView)
            : base(imageView)
        {
        }

        protected override bool GetBitmap(object value, out Bitmap bitmap)
        {
            var assetStream = this.GetStream(value);
            if (assetStream == null)
            {
                bitmap = null;
                return false;
            }

            var options = new BitmapFactory.Options { InPurgeable = true };
            bitmap = BitmapFactory.DecodeStream(assetStream, null, options);
            return true;
        }

        protected override void SetImageBitmap(AppCompatImageView imageView, Bitmap bitmap)
        {
#warning ASK STEVE @ SEQUENCE - WHY IS THIS NEEDED?
            var drawable = new BitmapDrawable(Resources.System, bitmap);
            imageView.SetImageDrawable(drawable);
        }

        protected abstract Stream GetStream(object value);
    }
}