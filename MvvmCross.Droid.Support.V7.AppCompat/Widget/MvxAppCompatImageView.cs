// MvxAppCompatImageView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Droid.Support.V7.AppCompat.Widget
{
    using System;

    using Android.Content;
    using Android.Graphics;
    using Android.OS;
    using Android.Runtime;
    using Android.Support.V7.Widget;
    using Android.Util;

    using MvvmCross.Binding.Droid.ResourceHelpers;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.Platform;

    [Register("mvvmcross.droid.support.v7.appcompat.widget.MvxAppCompatImageView")]
    public class MvxAppCompatImageView : AppCompatImageView
    {
        private IMvxImageHelper<Bitmap> _imageHelper;

        public MvxAppCompatImageView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            var typedArray = context.ObtainStyledAttributes(attrs, MvxAndroidBindingResource.Instance.ImageViewStylableGroupId);

            var numStyles = typedArray.IndexCount;
            for (var i = 0; i < numStyles; ++i)
            {
                var attributeId = typedArray.GetIndex(i);
                if (attributeId == MvxAndroidBindingResource.Instance.SourceBindId)
                {
                    this.ImageUrl = typedArray.GetString(attributeId);
                }
            }
            typedArray.Recycle();
        }

        public MvxAppCompatImageView(Context context)
            : base(context)
        {
        }

        protected MvxAppCompatImageView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public string ImageUrl
        {
            get
            {
                return this.ImageHelper?.ImageUrl;
            }
            set
            {
                if (this.ImageHelper == null)
                {
                    return;
                }
                this.ImageHelper.ImageUrl = value;
            }
        }

        public string DefaultImagePath
        {
            get
            {
                return this.ImageHelper.DefaultImagePath;
            }
            set
            {
                this.ImageHelper.DefaultImagePath = value;
            }
        }

        public string ErrorImagePath
        {
            get
            {
                return this.ImageHelper.ErrorImagePath;
            }
            set
            {
                this.ImageHelper.ErrorImagePath = value;
            }
        }

        protected IMvxImageHelper<Bitmap> ImageHelper
        {
            get
            {
                if (this._imageHelper == null)
                {
                    if (!Mvx.TryResolve(out this._imageHelper))
                    {
                        MvxTrace.Error("No IMvxImageHelper registered - you must provide an image helper before you can use a MvxImageView");
                    }
                    else
                    {
                        this._imageHelper.ImageChanged += this.ImageHelperOnImageChanged;
                    }
                }
                return this._imageHelper;
            }
        }

        public override void SetMaxHeight(int maxHeight)
        {
            if (this.ImageHelper != null)
            {
                this.ImageHelper.MaxHeight = maxHeight;
            }

            base.SetMaxHeight(maxHeight);
        }

        public override void SetMaxWidth(int maxWidth)
        {
            if (this.ImageHelper != null)
            {
                this.ImageHelper.MaxWidth = maxWidth;
            }

            base.SetMaxWidth(maxWidth);
        }

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
            using (var h = new Handler(Looper.MainLooper)) h.Post(() => { this.SetImageBitmap(mvxValueEventArgs.Value); });
        }
    }
}