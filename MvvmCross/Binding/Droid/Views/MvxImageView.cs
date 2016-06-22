// MvxImageView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Views
{
    using System;

    using Android.Content;
    using Android.Graphics;
    using Android.OS;
    using Android.Runtime;
    using Android.Util;
    using Android.Widget;

    using MvvmCross.Binding.Droid.ResourceHelpers;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.Platform;

    [Register("mvvmcross.binding.droid.views.MvxImageView")]
    public class MvxImageView
        : ImageView
    {
        private IMvxImageHelper<Bitmap> _imageHelper;

        public string ImageUrl
        {
            get
            {
                return ImageHelper?.ImageUrl;
            }
            set
            {
                if (ImageHelper == null)
                    return;
                ImageHelper.ImageUrl = value;
            }
        }

        public string DefaultImagePath
        {
            get { return ImageHelper.DefaultImagePath; }
            set { ImageHelper.DefaultImagePath = value; }
        }

        public string ErrorImagePath
        {
            get { return ImageHelper.ErrorImagePath; }
            set { ImageHelper.ErrorImagePath = value; }
        }

        public override void SetMaxHeight(int maxHeight)
        {
            if (ImageHelper != null)
                ImageHelper.MaxHeight = maxHeight;

            base.SetMaxHeight(maxHeight);
        }

        public override void SetMaxWidth(int maxWidth)
        {
            if (ImageHelper != null)
                ImageHelper.MaxWidth = maxWidth;

            base.SetMaxWidth(maxWidth);
        }

        protected IMvxImageHelper<Bitmap> ImageHelper
        {
            get
            {
                if (_imageHelper == null)
                {
                    if (!Mvx.TryResolve(out _imageHelper))
                    {
                        MvxTrace.Error(
                            "No IMvxImageHelper registered - you must provide an image helper before you can use a MvxImageView");
                    }
                    else
                    {
                        _imageHelper.ImageChanged += ImageHelperOnImageChanged;
                    }
                }
                return _imageHelper;
            }
        }

        public MvxImageView(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
            Init(context, attrs);
        }

        public MvxImageView(Context context, IAttributeSet attrs)
            : this(context, attrs, 0)
        { }

        public MvxImageView(Context context)
            : this(context, null)
        { }

        protected MvxImageView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        { }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_imageHelper != null)
                {
                    _imageHelper.ImageChanged -= ImageHelperOnImageChanged;
                    _imageHelper?.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        private void ImageHelperOnImageChanged(object sender, MvxValueEventArgs<Bitmap> mvxValueEventArgs)
        {
            using (var h = new Handler(Looper.MainLooper))
                h.Post(() =>
                {
                    SetImageBitmap(mvxValueEventArgs.Value);
                });
        }

        private void Init(Context context, IAttributeSet attrs)
        {
            var typedArray = context.ObtainStyledAttributes(attrs, MvxAndroidBindingResource.Instance.ImageViewStylableGroupId);

            int numStyles = typedArray.IndexCount;
            for (var i = 0; i < numStyles; ++i)
            {
                int attributeId = typedArray.GetIndex(i);
                if (attributeId == MvxAndroidBindingResource.Instance.SourceBindId)
                {
                    ImageUrl = typedArray.GetString(attributeId);
                }
            }
            typedArray.Recycle();
        }

        public override void SetImageBitmap (Bitmap bm)
        {
            if (Handle != IntPtr.Zero)
            {
                if (bm != null && (bm.Handle == IntPtr.Zero || bm.IsRecycled))
                {
                    // Don't try to update disposed or recycled bitmap
                    return;
                }
                base.SetImageBitmap (bm);
            }
        }
    }
}