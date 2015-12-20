// ImageElement.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Provider;
using Android.Views;
using Android.Widget;

namespace CrossUI.Droid.Dialog.Elements
{
#warning Not yet ported - not sure I'd use this version... too hard-coded to ImageViews?

    public class ImageElement : Element
    {
        // Height for rows
        private const int dimx = 48;

        private const int dimy = 44;

        // radius for rounding
        private const int roundPx = 12;

        public readonly ImageView Value;
        private ImageView scaled;

        public ImageElement(ImageView image = null)
            : base(string.Empty)
        {
            if (image == null)
            {
                Value = MakeEmpty();
                scaled = Value;
            }
            else
            {
                Value = image;
                if (image.Drawable != null)
                    scaled = Scale(Value);
            }
        }

        private static ImageView MakeEmpty()
        {
            return new ImageView(null);
        }

        private ImageView Scale(ImageView source)
        {
            var drawable = (BitmapDrawable)source.Drawable;
            var bitmap = drawable.Bitmap;
            var bMapScaled = Bitmap.CreateScaledBitmap(bitmap, dimx, dimy, true);
            source.SetImageBitmap(bMapScaled);
            return source;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                scaled?.Dispose();
                Value?.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override View GetViewImpl(Context context, ViewGroup parent)
        {
            if (scaled == null)
                scaled = Scale(Value);

            Click = delegate { SelectImage(); };

            var view = new RelativeLayout(context);

            var parms = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                                                        ViewGroup.LayoutParams.WrapContent);
            parms.SetMargins(5, 2, 5, 2);
            parms.AddRule(LayoutRules.AlignParentLeft);

            // SEC bug fix, not yet submitted to Kenny.  Getting exception "specified view already has a parent"
            if (scaled.Parent != view)
                view.AddView(scaled, parms);

            return view;
        }

        private void SelectImage()
        {
            var activity = (Activity)Context;
            var intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.InternalContentUri);
            activity.StartActivity(intent);
        }
    }
}