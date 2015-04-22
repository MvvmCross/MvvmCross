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
    [Register ("cirrious.mvvmcross.binding.droid.views.MvxImageView")]
    public class MvxImageView
        : ImageView
    {
        private readonly IMvxImageHelper<Bitmap> _imageHelper;

        public MvxImageView(Context context, IAttributeSet attrs)
            : base(context, attrs)
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

		protected MvxImageView(IntPtr javaReference, JniHandleOwnership transfer)
			: base(javaReference, transfer)
	    {
	    }

        public string ImageUrl
        {
            get
            {
                if (_imageHelper == null)
                    return null;
                return _imageHelper.ImageUrl;
            }
            set
            {
                if (_imageHelper == null)
                    return;
                _imageHelper.ImageUrl = value;
            }
        }

        public string DefaultImagePath
        {
            get { return _imageHelper.DefaultImagePath; }
            set { _imageHelper.DefaultImagePath = value; }
        }

        public string ErrorImagePath
        {
            get { return _imageHelper.ErrorImagePath; }
            set { _imageHelper.ErrorImagePath = value; }
        }

        [Obsolete("Use ImageUrl instead")]
        public string HttpImageUrl
        {
            get { return ImageUrl; }
            set { ImageUrl = value; }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_imageHelper != null)
                    _imageHelper.Dispose();
            }

            base.Dispose(disposing);
        }

        private void ImageHelperOnImageChanged(object sender, MvxValueEventArgs<Bitmap> mvxValueEventArgs)
        {
            SetImageBitmap(mvxValueEventArgs.Value);
        }
    }
}