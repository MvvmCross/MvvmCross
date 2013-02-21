using System;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Interfaces.Core;
using Cirrious.CrossCore.Interfaces.Platform;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public class MvxImageViewWrapper 
		: IDisposable
		, IMvxServiceConsumer
    {
        private Func<UIImageView> _imageView;
        private IMvxImageHelper<UIImage> _imageHelper;
		
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
            _imageHelper = this.GetService<IMvxImageHelper<UIImage>>();
            _imageHelper.ImageChanged += ImageHelperOnImageChanged;
        }

        ~MvxImageViewWrapper()
        {
            Dispose(false);
        }

        public void Dispose ()
        {
            Dispose (true);
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