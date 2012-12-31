#region Copyright

// <copyright file="MvxTouchImageView.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System.Drawing;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Plugins.DownloadCache;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public class MvxTouchImageView
        : UIImageView
    {
        private MvxDynamicImageHelper<UIImage> _imageHelper;

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

        #region constructors

        public MvxTouchImageView()
        {
            InitialiseImageHelper();
        }

        public MvxTouchImageView(RectangleF frame)
            : base(frame)
        {
            InitialiseImageHelper();
        }

        #endregion

        private void InitialiseImageHelper()
        {
            _imageHelper = new MvxDynamicImageHelper<UIImage>();
            _imageHelper.ImageChanged += ImageHelperOnImageChanged;
        }

        private void ImageHelperOnImageChanged(object sender, MvxValueEventArgs<UIImage> mvxValueEventArgs)
        {
            Image = mvxValueEventArgs.Value;
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