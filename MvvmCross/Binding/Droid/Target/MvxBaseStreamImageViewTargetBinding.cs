// MvxBaseStreamImageViewTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.IO;

using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;

namespace MvvmCross.Binding.Droid.Target
{
    public abstract class MvxBaseStreamImageViewTargetBinding
        : MvxBaseImageViewTargetBinding
    {
        protected MvxBaseStreamImageViewTargetBinding(ImageView imageView)
            : base(imageView)
        {
        }

        protected override bool GetBitmap(object value, out Bitmap bitmap)
        {
            var assetStream = GetStream(value);
            if (assetStream == null)
            {
                bitmap = null;
                return false;
            }

            var options = new BitmapFactory.Options { InPurgeable = true };
            bitmap = BitmapFactory.DecodeStream(assetStream, null, options);
            return true;
        }

        protected override void SetImageBitmap(ImageView imageView, Bitmap bitmap)
        {
            var drawable = new BitmapDrawable(Resources.System, bitmap);
            imageView.SetImageDrawable(drawable);
        }

        protected abstract Stream GetStream(object value);
    }
}