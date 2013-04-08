
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
using Cirrious.CrossCore;
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

			this.DelayBind (() =>
			{

                var bindings = this.CreateBindingSet<TwitterCell, Tweet>();
                bindings.Bind(_imageHelper).To(tweet => tweet.ProfileImageUrl);
                bindings.Bind(PersonLabel).To(tweet => tweet.Author);
                bindings.Bind(WhenLabel).To(tweet => tweet.Timestamp).WithConversion("TimeAgo");
                bindings.Bind(MainLabel).To(tweet => tweet.Title);
                bindings.Apply();

                /*
				this.CreateBinding(_imageHelper).To((Tweet tweet) => tweet.ProfileImageUrl).Apply();
				this.CreateBinding(PersonLabel).To((Tweet tweet) => tweet.Author).Apply();
				this.CreateBinding(WhenLabel).To((Tweet tweet) => tweet.Timestamp).WithConversion("TimeAgo").Apply();
				this.CreateBinding(MainLabel).To((Tweet tweet) => tweet.Title).Apply();
                 */
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

