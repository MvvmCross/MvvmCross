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
                return this.ImageHelper?.ImageUrl;
            }
            set
            {
                if (this.ImageHelper == null)
                    return;
                this.ImageHelper.ImageUrl = value;
            }
        }

        public string DefaultImagePath
        {
            get { return this.ImageHelper.DefaultImagePath; }
            set { this.ImageHelper.DefaultImagePath = value; }
        }

        public string ErrorImagePath
        {
            get { return this.ImageHelper.ErrorImagePath; }
            set { this.ImageHelper.ErrorImagePath = value; }
        }

        public override void SetMaxHeight(int maxHeight)
        {
            if (this.ImageHelper != null)
                this.ImageHelper.MaxHeight = maxHeight;

            base.SetMaxHeight(maxHeight);
        }

        public override void SetMaxWidth(int maxWidth)
        {
            if (this.ImageHelper != null)
                this.ImageHelper.MaxWidth = maxWidth;

            base.SetMaxWidth(maxWidth);
        }

        protected IMvxImageHelper<Bitmap> ImageHelper
        {
            get
            {
                if (this._imageHelper == null)
                {
                    if (!Mvx.TryResolve(out this._imageHelper))
                    {
                        MvxTrace.Error(
                            "No IMvxImageHelper registered - you must provide an image helper before you can use a MvxImageView");
                    }
                    else
                    {
                        this._imageHelper.ImageChanged += this.ImageHelperOnImageChanged;
                    }
                }
                return this._imageHelper;
            }
        }

        public MvxImageView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            var typedArray = context.ObtainStyledAttributes(attrs,
                                                            MvxAndroidBindingResource.Instance
                                                                .ImageViewStylableGroupId);

            int numStyles = typedArray.IndexCount;
            for (var i = 0; i < numStyles; ++i)
            {
                int attributeId = typedArray.GetIndex(i);
                if (attributeId == MvxAndroidBindingResource.Instance.SourceBindId)
                {
                    this.ImageUrl = typedArray.GetString(attributeId);
                }
            }
            typedArray.Recycle();
        }

        public MvxImageView(Context context)
            : base(context)
        { }

        protected MvxImageView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        { }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._imageHelper?.Dispose();
            }

            base.Dispose(disposing);
        }

        private void ImageHelperOnImageChanged(object sender, MvxValueEventArgs<Bitmap> mvxValueEventArgs)
        {
            using (var h = new Handler(Looper.MainLooper))
                h.Post(() =>
                {
                    this.SetImageBitmap(mvxValueEventArgs.Value);
                });
        }

        public override void SetImageBitmap (Bitmap bm)
        {
            if (Handle != IntPtr.Zero)
            {
                base.SetImageBitmap (bm);
            }
        }
    }
}