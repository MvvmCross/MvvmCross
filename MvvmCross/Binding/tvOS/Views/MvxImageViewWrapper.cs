// MvxImageViewWrapper.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Platform;
using UIKit;

namespace MvvmCross.Binding.tvOS.Views
{
    public class MvxImageViewWrapper
        : IDisposable
    {
        private readonly Func<UIImageView> _imageView;
        private readonly IMvxImageHelper<UIImage> _imageHelper;

        public string ImageUrl
        {
            get { return _imageHelper.ImageUrl; }
            set { _imageHelper.ImageUrl = value; }
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

        public MvxImageViewWrapper(Func<UIImageView> imageView)
        {
            _imageView = imageView;
            _imageHelper = Mvx.Resolve<IMvxImageHelper<UIImage>>();
            _imageHelper.ImageChanged += ImageHelperOnImageChanged;
        }

        ~MvxImageViewWrapper()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void ImageHelperOnImageChanged(object sender, MvxValueEventArgs<UIImage> mvxValueEventArgs)
        {
            var imageView = _imageView();
            if (imageView != null && mvxValueEventArgs.Value != null)
                imageView.Image = mvxValueEventArgs.Value;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _imageHelper.Dispose();
            }
        }
    }
}