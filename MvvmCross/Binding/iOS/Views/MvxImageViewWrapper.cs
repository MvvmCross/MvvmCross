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

namespace MvvmCross.Binding.iOS.Views
{
    public class MvxImageViewWrapper
        : IDisposable
    {
        private readonly IMvxImageHelper<UIImage> _imageHelper;
        private readonly Func<UIImageView> _imageView;

        public MvxImageViewWrapper(Func<UIImageView> imageView)
        {
            _imageView = imageView;
            _imageHelper = Mvx.Resolve<IMvxImageHelper<UIImage>>();
            _imageHelper.ImageChanged += ImageHelperOnImageChanged;
        }

        public string ImageUrl
        {
            get => _imageHelper.ImageUrl;
            set => _imageHelper.ImageUrl = value;
        }

        public string DefaultImagePath
        {
            get => _imageHelper.DefaultImagePath;
            set => _imageHelper.DefaultImagePath = value;
        }

        public string ErrorImagePath
        {
            get => _imageHelper.ErrorImagePath;
            set => _imageHelper.ErrorImagePath = value;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MvxImageViewWrapper()
        {
            Dispose(false);
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
                _imageHelper.Dispose();
        }
    }
}