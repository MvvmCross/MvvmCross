// MvxImageViewWrapper.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Touch.Views
{
    using System;

    using MvvmCross.Platform;
    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.Platform;

    using UIKit;

    public class MvxImageViewWrapper
        : IDisposable
    {
        private readonly Func<UIImageView> _imageView;
        private readonly IMvxImageHelper<UIImage> _imageHelper;

        public string ImageUrl
        {
            get { return this._imageHelper.ImageUrl; }
            set { this._imageHelper.ImageUrl = value; }
        }

        public string DefaultImagePath
        {
            get { return this._imageHelper.DefaultImagePath; }
            set { this._imageHelper.DefaultImagePath = value; }
        }

        public string ErrorImagePath
        {
            get { return this._imageHelper.ErrorImagePath; }
            set { this._imageHelper.ErrorImagePath = value; }
        }

        public MvxImageViewWrapper(Func<UIImageView> imageView)
        {
            this._imageView = imageView;
            this._imageHelper = Mvx.Resolve<IMvxImageHelper<UIImage>>();
            this._imageHelper.ImageChanged += this.ImageHelperOnImageChanged;
        }

        ~MvxImageViewWrapper()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void ImageHelperOnImageChanged(object sender, MvxValueEventArgs<UIImage> mvxValueEventArgs)
        {
            var imageView = this._imageView();
            if (imageView != null && mvxValueEventArgs.Value != null)
                imageView.Image = mvxValueEventArgs.Value;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._imageHelper.Dispose();
            }
        }
    }
}