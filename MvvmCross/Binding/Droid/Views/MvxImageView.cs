// MvxImageView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Droid.ResourceHelpers;
using System;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    [Register("cirrious.mvvmcross.binding.droid.views.MvxImageView")]
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
        { }

        protected MvxImageView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        { }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _imageHelper?.Dispose();
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
    }
}