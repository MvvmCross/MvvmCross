// MvxImageView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Droid.ResourceHelpers;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    [Register("cirrious.mvvmcross.binding.droid.views.MvxImageView")]
    public class MvxImageView
        : ImageView
    {
        private IMvxImageHelper<Bitmap> _imageHelper;

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
                    ImageUrl = typedArray.GetString(attributeId);
                }
            }
            typedArray.Recycle();
        }

        public MvxImageView(Context context)
            : base(context)
        {
        }

		protected MvxImageView(IntPtr javaReference, JniHandleOwnership transfer)
			: base(javaReference, transfer)
	    {
	    }

        public string ImageUrl
        {
            get
            {
                if (ImageHelper == null)
                    return null;
                return ImageHelper.ImageUrl;
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

        [Obsolete("Use ImageUrl instead")]
        public string HttpImageUrl
        {
            get { return ImageUrl; }
            set { ImageUrl = value; }
        }

        protected IMvxImageHelper<Bitmap> ImageHelper
        {
            get
            {
                if (!Mvx.TryResolve(out _imageHelper))
                {
                    MvxTrace.Error(
                        "No IMvxImageHelper registered - you must provide an image helper before you can use a MvxImageView");
                }
                else
                {
                    ImageHelper.ImageChanged += ImageHelperOnImageChanged;
                }
                return _imageHelper;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (ImageHelper != null)
                    ImageHelper.Dispose();
            }

            base.Dispose(disposing);
        }

        private void ImageHelperOnImageChanged(object sender, MvxValueEventArgs<Bitmap> mvxValueEventArgs)
        {
            SetImageBitmap(mvxValueEventArgs.Value);
        }
    }
}