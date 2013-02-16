using System;
using System.Drawing;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Plugins.DownloadCache;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    [Register("MvxImageView")]
    public class MvxImageView
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
		
        public MvxImageView()
        {
            InitialiseImageHelper();
        }
		
        public MvxImageView(IntPtr handle)
            : base(handle)
        {
            InitialiseImageHelper();
        }
		
        public MvxImageView(RectangleF frame)
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