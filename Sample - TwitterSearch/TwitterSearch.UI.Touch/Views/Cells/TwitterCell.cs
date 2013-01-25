
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Plugins.DownloadCache;
using Cirrious.MvvmCross.Platform;
using TwitterSearch.Core.Models;

namespace TwitterSearch.UI.Touch
{
	[Register("TwitterCell")]
	public partial class TwitterCell : MvxBaseBindableTableViewCell
	{
		public static readonly NSString CellIdentifier = new NSString("TwitterCell");

		private const string BindingText = @"
                        Author Author,
                        Body Title,
                        When Timestamp, Converter=TimeAgo,
                        ImageUrl ProfileImageUrl
        ";
		private MvxDynamicImageHelper<UIImage> _imageHelper;

		public static float CellHeight (object item)
		{
			return CellHeight((Tweet)item);	
		}

		private static UIFont MainBodyFont = UIFont.SystemFontOfSize(14.0f);
		private const float MagicAdjustment = 80f;
		private const float BodyWidthIphone = 234f;
		private const float BodyWidthIpad = 362f;

		private static float CellHeight (Tweet tweet)
		{
			var text = new NSString(tweet.Title);
			var restrictTo = new SizeF(AppDelegate.IsPad ? BodyWidthIpad : BodyWidthIphone, 999999);
			return text.StringSize(MainBodyFont, restrictTo).Height + MagicAdjustment;
		}

		public TwitterCell () 
			: base (BindingText)
		{
			Initialise();
		}

		public TwitterCell (IntPtr handle) 
			: base (BindingText, handle)
		{
			Initialise();
		}

		private void Initialise()
		{
			_imageHelper = new MvxDynamicImageHelper<UIImage>();
			_imageHelper.ImageChanged += ImageHelperOnImageChanged;
		}

		protected override void Dispose (bool disposing)
		{
			if (disposing) {
				if (_imageHelper != null)
				{
					_imageHelper.ImageChanged -= ImageHelperOnImageChanged;
					_imageHelper.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		private void ImageHelperOnImageChanged(object sender, MvxValueEventArgs<UIImage> mvxValueEventArgs)
		{
			if (mvxValueEventArgs.Value != null)
			 	ProfileImageView.Image = mvxValueEventArgs.Value;
		}

		public string ImageUrl {
			get { return _imageHelper.ImageUrl; }
			set { _imageHelper.ImageUrl = value; }
		}

		public string Author {
			get { return PersonLabel.Text; }
			set { PersonLabel.Text = value; }
		}

		public string When {
			get { return WhenLabel.Text; }
			set { WhenLabel.Text = value; }
		}

		public string Body {
			get { return MainLabel.Text; }
			set { MainLabel.Text = value; }
		}
	}
}

