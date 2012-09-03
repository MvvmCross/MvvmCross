using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Interfaces.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Commands;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Platform.Images;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
	public class MvxTouchImageView
		: UIImageView
	{
        private MvxDynamicImageHelper<UIImage> _imageHelper;

		public string HttpImageUrl
		{
			get { return _imageHelper.HttpImageUrl; }
			set { _imageHelper.HttpImageUrl = value; }
		}

        public MvxDynamicImageHelper<UIImage> ImageHelper
        {
            get { return _imageHelper; }
        }

		#region constructors
		public MvxTouchImageView ()
			: base()
		{
            InitialiseImageHelper();
		}

		public MvxTouchImageView (RectangleF frame)
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

