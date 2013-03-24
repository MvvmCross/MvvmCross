
using System;
using System.Drawing;
using Cirrious.CrossCore.Core;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Plugins.DownloadCache;
using Cirrious.MvvmCross.Platform;
using TwitterSearch.Core.Models;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.Binding.ExtensionMethods;

namespace TwitterSearch.UI.Touch
{
	[Register("TwitterCell")]
	public partial class TwitterCell : MvxTableViewCell
	{
		public static readonly NSString CellIdentifier = new NSString("TwitterCell");

		private MvxImageViewLoader _imageHelper;

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
			: base ()
		{
			Initialise();
		}

		public TwitterCell (IntPtr handle) 
			: base (handle)
		{
			Initialise();
		}

		private void Initialise()
		{
			_imageHelper = new MvxImageViewLoader(() => ProfileImageView);

			this.DelayBind (() => {
				this.Bind(_imageHelper, (Tweet tweet) => tweet.ProfileImageUrl);
				this.Bind(PersonLabel, (Tweet tweet) => tweet.Author);
				this.Bind(WhenLabel, (Tweet tweet) => tweet.Timestamp, "TimeAgo");
				this.Bind(MainLabel, (Tweet tweet) => tweet.Title);
			});
		}

		protected override void Dispose (bool disposing)
		{
			if (disposing) {
				if (_imageHelper != null)
				{
					_imageHelper.Dispose();
				}
			}
			base.Dispose(disposing);
		}
	}
}

