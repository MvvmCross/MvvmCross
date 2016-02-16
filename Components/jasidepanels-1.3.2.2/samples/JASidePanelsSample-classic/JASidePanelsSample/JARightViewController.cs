using System;
#if __UNIFIED__
using CoreGraphics;
using UIKit;
#else
using System.Drawing;
using MonoTouch.UIKit;
using CGRect = global::System.Drawing.RectangleF;
using CGPoint = global::System.Drawing.PointF;
#endif

using JASidePanels;

namespace JASidePanelsSample
{
	public class JARightViewController : JALeftViewController
	{
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			View.BackgroundColor = UIColor.Red;
			label.Text = "Right Panel";
			label.SizeToFit();
			hide.Frame = new CGRect(View.Bounds.Size.Width - 220.0f, 70.0f, 200.0f, 40.0f);
			hide.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleBottomMargin;
			show.Frame = hide.Frame;
			show.AutoresizingMask = hide.AutoresizingMask;

			removeRightPanel.Hidden = true;
			addRightPanel.Hidden = true;
			changeCenterPanel.Hidden = true;
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			label.Center = new CGPoint ((int)((View.Bounds.Size.Width - this.GetSidePanelController ().RightVisibleWidth) + this.GetSidePanelController ().RightVisibleWidth / 2.0f), 25.0f);
		}
	}
}
