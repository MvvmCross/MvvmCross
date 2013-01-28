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
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Plugins.DownloadCache;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public class MvxImageView
        : ImageView
    {
        static MvxImageView()
        {
            Cirrious.MvvmCross.Plugins.DownloadCache.PluginLoader.Instance.EnsureLoaded();
        }

        private readonly MvxDynamicImageHelper<Bitmap> _imageHelper;

        public MvxImageView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            _imageHelper = new MvxDynamicImageHelper<Bitmap>();
            _imageHelper.ImageChanged += ImageHelperOnImageChanged;
            var typedArray = context.ObtainStyledAttributes(attrs,
                                                            MvxAndroidBindingResource.Instance
                                                                                     .HttpImageViewStylableGroupId);

            int numStyles = typedArray.IndexCount;
            for (var i = 0; i < numStyles; ++i)
            {
                int attributeId = typedArray.GetIndex(i);
                if (attributeId == MvxAndroidBindingResource.Instance.HttpSourceBindId)
                {
                    ImageUrl = typedArray.GetString(attributeId);
                }
            }
            typedArray.Recycle();
        }

        public string ImageUrl
        {
            get { return Image.ImageUrl; }
            set { Image.ImageUrl = value; }
        }

        [Obsolete("Use ImageUrl instead")]
        public string HttpImageUrl
        {
            get { return Image.HttpImageUrl; }
            set { Image.HttpImageUrl = value; }
        }

        public MvxImageView(Context context)
            : base(context)
        {
        }

        protected MvxImageView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public MvxDynamicImageHelper<Bitmap> Image
        {
            get { return _imageHelper; }
        }

        private void ImageHelperOnImageChanged(object sender, MvxValueEventArgs<Bitmap> mvxValueEventArgs)
        {
            SetImageBitmap(mvxValueEventArgs.Value);
        }
    }
}