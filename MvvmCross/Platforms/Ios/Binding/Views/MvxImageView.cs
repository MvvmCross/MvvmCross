// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MvvmCross.Platform.Ios.Binding.Views
{
    [Register("MvxImageView")]
    public class MvxImageView
        : UIImageView
    {
        private MvxImageViewLoader _imageHelper;
        public event EventHandler ImageChanged;

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

        public MvxImageView()
        {
            InitializeImageHelper();
        }

        public MvxImageView(IntPtr handle)
            : base(handle)
        {
            InitializeImageHelper();
        }

        public MvxImageView(CGRect frame, Action imageChanged = null)
            : base(frame)
        {
            InitializeImageHelper(imageChanged);
        }

        private void InitializeImageHelper(Action imageChanged = null)
        {
            _imageHelper = new MvxImageViewLoader(() => this, AfterImageChanged);
        }

        protected virtual void AfterImageChanged()
        {
            ImageChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _imageHelper.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
