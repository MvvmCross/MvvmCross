// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MvvmCross.Platform.Tvos.Binding.Views
{
    [Register("MvxImageView")]
    public class MvxImageView
        : UIImageView
    {
        private MvxImageViewLoader _imageHelper;

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

        public MvxImageView(Action afterImageChangeAction = null)
        {
            InitializeImageHelper(afterImageChangeAction);
        }

        public MvxImageView(IntPtr handle)
            : base(handle)
        {
            InitializeImageHelper();
        }

        public MvxImageView(CGRect frame, Action afterImageChangeAction = null)
            : base(frame)
        {
            InitializeImageHelper(afterImageChangeAction);
        }

        private void InitializeImageHelper(Action afterImageChangeAction = null)
        {
            _imageHelper = new MvxImageViewLoader(() => this, afterImageChangeAction);
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
