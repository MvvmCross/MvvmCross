// MvxImageView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CoreGraphics;
using Foundation;
using System;
using UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
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