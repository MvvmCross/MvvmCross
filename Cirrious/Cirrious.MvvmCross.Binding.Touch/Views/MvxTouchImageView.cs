// MvxTouchImageView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Drawing;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Plugins.DownloadCache;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
	public class MvxImageViewWrapper : IDisposable
	{
		private Func<UIImageView> _imageView;
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
		
		public MvxImageViewWrapper(Func<UIImageView> imageView)
		{
			_imageView = imageView;
			_imageHelper = new MvxDynamicImageHelper<UIImage>();
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

	[Obsolete("Use MvxImageView")]
    public class MvxTouchImageView
        : MvxImageView
    {
        #region constructors

        public MvxTouchImageView()
        {
        }

		public MvxTouchImageView(IntPtr handle)
			: base(handle)
		{
		}

        public MvxTouchImageView(RectangleF frame)
            : base(frame)
        {
        }

        #endregion
    }
}