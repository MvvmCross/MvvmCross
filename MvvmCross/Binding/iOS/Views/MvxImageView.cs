// MvxImageView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.iOS.Views
{
    using System;

    using CoreGraphics;

    using Foundation;

    using UIKit;

    [Register("MvxImageView")]
    public class MvxImageView
        : UIImageView
    {
        private MvxImageViewLoader _imageHelper;

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

        public MvxImageView(Action imageChanged = null)
        {
            this.InitializeImageHelper(imageChanged);
        }

        public MvxImageView(IntPtr handle)
            : base(handle)
        {
            this.InitializeImageHelper();
        }

        public MvxImageView(CGRect frame, Action imageChanged = null)
            : base(frame)
        {
            this.InitializeImageHelper(imageChanged);
        }

        private void InitializeImageHelper(Action imageChanged = null)
        {
            this._imageHelper = new MvxImageViewLoader(() => this, imageChanged);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._imageHelper.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}