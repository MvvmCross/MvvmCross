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
	public class JALeftViewController : JADebugViewController
	{
		protected UILabel label;
		protected UIButton hide;
		protected UIButton show;
		protected UIButton removeRightPanel;
		protected UIButton addRightPanel;
		protected UIButton changeCenterPanel;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			View.BackgroundColor = UIColor.Blue;

			label = new UILabel ();
			label.Font = UIFont.BoldSystemFontOfSize (20);
			label.TextColor = UIColor.White;
			label.BackgroundColor = UIColor.Clear;
			label.Text = "Left Panel";
			label.SizeToFit();
			label.AutoresizingMask = UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleBottomMargin;
			View.AddSubview (label);


			hide = new UIButton (UIButtonType.RoundedRect);
			hide.Frame = new CGRect (20.0f, 70.0f, 200.0f, 40.0f);
			hide.AutoresizingMask = UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleBottomMargin;
			hide.SetTitle ("Hide Center", UIControlState.Normal);
			hide.TouchUpInside += (sender, e) => {
				this.GetSidePanelController().SetCenterPanelHidden(true, true, 0.2f);
				hide.Hidden = true;
				show.Hidden = false;
			};
			View.AddSubview (hide);

			show = new UIButton (UIButtonType.RoundedRect);
			show.Frame = hide.Frame;
			show.AutoresizingMask = UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleBottomMargin;
			show.SetTitle ("Show Center", UIControlState.Normal);
			show.TouchUpInside += (sender, e) => {
				this.GetSidePanelController().SetCenterPanelHidden(false, true, 0.2f);
				hide.Hidden = false;
				show.Hidden = true;
			};
			show.Hidden = true;
			View.AddSubview (show);


			removeRightPanel = new UIButton (UIButtonType.RoundedRect);
			removeRightPanel.Frame = new CGRect (20.0f, 120.0f, 200.0f, 40.0f);
			removeRightPanel.AutoresizingMask = UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleBottomMargin;
			removeRightPanel.SetTitle ("Remove Right Panel", UIControlState.Normal);
			removeRightPanel.TouchUpInside += (sender, e) => {
				this.GetSidePanelController().RightPanel = null;
				removeRightPanel.Hidden = true;
				addRightPanel.Hidden = false;
			};
			View.AddSubview (removeRightPanel);

			addRightPanel = new UIButton (UIButtonType.RoundedRect);
			addRightPanel.Frame = removeRightPanel.Frame;
			addRightPanel.AutoresizingMask = UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleBottomMargin;
			addRightPanel.SetTitle ("Add Right Panel", UIControlState.Normal);
			addRightPanel.TouchUpInside += (sender, e) => {
				this.GetSidePanelController().RightPanel = new JARightViewController();
				removeRightPanel.Hidden = false;
				addRightPanel.Hidden = true;
			};
			addRightPanel.Hidden = true;
			View.AddSubview (addRightPanel);


			changeCenterPanel = new UIButton (UIButtonType.RoundedRect);
			changeCenterPanel.Frame = new CGRect (20.0f, 170.0f, 200.0f, 40.0f);
			changeCenterPanel.AutoresizingMask = UIViewAutoresizing.FlexibleRightMargin;
			changeCenterPanel.SetTitle ("Change Center Panel", UIControlState.Normal);
			changeCenterPanel.TouchUpInside += (sender, e) => {
				this.GetSidePanelController().CenterPanel = new UINavigationController(new JACenterViewController());
			};
			View.AddSubview (changeCenterPanel);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			label.Center = new CGPoint ((int)(this.GetSidePanelController ().LeftVisibleWidth / 2.0f), 25.0f);
		}
	}
}
