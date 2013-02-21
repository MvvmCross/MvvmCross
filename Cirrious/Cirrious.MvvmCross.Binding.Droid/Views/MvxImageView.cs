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
using Cirrious.CrossCore.Interfaces.Core;
using Cirrious.CrossCore.Interfaces.Platform;
using Cirrious.CrossCore.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public class MvxImageView
        : ImageView
        , IMvxServiceConsumer
    {
        private readonly IMvxImageHelper<Bitmap> _imageHelper;

        public MvxImageView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            _imageHelper = this.GetService<IMvxImageHelper<Bitmap>>();
            _imageHelper.ImageChanged += ImageHelperOnImageChanged;
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

        public string ImageUrl
        {
            get { return Image.ImageUrl; }
            set { Image.ImageUrl = value; }
        }

        [Obsolete("Use ImageUrl instead")]
        public string HttpImageUrl
        {
            get { return Image.ImageUrl; }
            set { Image.ImageUrl = value; }
        }

        public MvxImageView(Context context)
            : base(context)
        {
        }

        protected MvxImageView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public IMvxImageHelper<Bitmap> Image
        {
            get { return _imageHelper; }
        }

        private void ImageHelperOnImageChanged(object sender, MvxValueEventArgs<Bitmap> mvxValueEventArgs)
        {
            SetImageBitmap(mvxValueEventArgs.Value);
        }
    }
}