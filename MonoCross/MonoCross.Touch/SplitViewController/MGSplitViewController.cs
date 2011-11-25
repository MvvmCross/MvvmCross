using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;

namespace MonoCross.Touch
{
	public enum MGSplitViewDividerStyle
	{
		Thin,
		PaneSplitter
	}
	
	/// <summary>
	/// The orientation in which the master pane will render as part of the split view controller.
	/// If the master pane does not render within the split view, it will render as a popover.
	/// </summary>
	public enum MasterOrientation
	{
		None = 0,
		Portrait = 1,
		Landscape = 2
	}

	
	public class MGSplitViewControllerDelegate
	{
		public virtual void WillPresentViewController(UIViewController controller)
		{
		}
		
		public virtual void WillChangeSplitOrientationToVertical(bool isVertical)
		{
		}
		
		public virtual void WillHideViewController(UIViewController masterViewController, UIBarButtonItem UIBarButtonItem, UIPopoverController popoverController)
		{
		}

		public virtual void WillShowViewController(UIViewController masterViewController, UIBarButtonItem UIBarButtonItem)
		{
		}
		
		public virtual void InvalidatingBarButtonItem(UIBarButtonItem barButtonItem)
		{
		}
			
		public virtual void WillMoveSplitToPosition(float position)
		{
		}
		
		public virtual float ConstrainSplitPosition(float newSize, SizeF fullSize)
		{
			return newSize;
		}
	}
	
	public class MGSplitViewController : UIViewController
	{
		public float DefaultThinWidth { get { return 1.0f; } }
		public float DefaultThickWidth { get { return 12.0f; } }
		
		public float SplitPosition { get; internal set; }
		public float SplitWidth { get; internal set; }
		public bool Vertical { get; internal set; }
		public bool MasterBeforeDetail { get; set; }

		public String MasterButtonText {
			get { return _barButtonItemText; } 
			set { _barButtonItemText = value; }
		}

		internal bool AdjustingMaster { get; set; }
		internal MGSplitDividerView DividerView { get; set; }
		internal MGSplitViewDividerStyle DividerStyle { get; set; }
		
		private float _defaultSplitPosition = 320.0f;
		private float _defaultCornerRadius = 5.0f;
		private UIColor _defaultCornerColor = UIColor.Black;
		
		private float _panesplitterCornerRadius = 0.0f;
		
		private float _minViewWidth	= 200.0f;
		
		private string _animationChangeSplitOrientation = "ChangeSplitOrientation";	// Animation ID for internal use.
		private string _animationChangeSubviewsOrder = "ChangeSubviewsOrder";	// Animation ID for internal use.

		private UIViewController _masterViewController;
		private UIViewController _detailViewController;
		private UIPopoverController _hiddenPopoverController;
		
		private bool _showsMasterInPortrait = true;
		private bool _showsMasterInLandscape = true;
		private bool _reconfigurePopup = false;
		private string _barButtonItemText = "Master";
		private UIBarButtonItem _barButtonItem;
		private MGSplitCornersView[] _cornerViews;
		
		private MGSplitViewControllerDelegate _splitViewDelegate = null;
		
		string NameOfInterfaceOrientation(UIInterfaceOrientation orientation)
		{
			string orientationName = null;
			switch (orientation) {
				case UIInterfaceOrientation.Portrait:
					orientationName = "Portrait"; // Home button at bottom
					break;
				case UIInterfaceOrientation.PortraitUpsideDown:
					orientationName = "Portrait (Upside Down)"; // Home button at top
					break;
				case UIInterfaceOrientation.LandscapeLeft:
					orientationName = "Landscape (Left)"; // Home button on left
					break;
				case UIInterfaceOrientation.LandscapeRight:
					orientationName = "Landscape (Right)"; // Home button on right
					break;
				default:
					break;
			}
		
			return orientationName;
		}
		
		bool IsLargeFormFactor
		{
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad; }
		}
	
		bool IsLandscape(UIInterfaceOrientation orientation)
		{
			return orientation == UIInterfaceOrientation.LandscapeLeft || orientation == UIInterfaceOrientation.LandscapeRight;
		}

		bool IsLandscape()
		{
			return IsLandscape(this.InterfaceOrientation);
		}
		
		bool ShouldShowMaster(UIInterfaceOrientation orientation)
		{
			// Returns true if master view should be shown directly embedded in the splitview, instead of hidden in a popover.
			return IsLandscape() ? _showsMasterInLandscape : _showsMasterInPortrait;
		}
		
		bool ShouldShowMaster()
		{
			return ShouldShowMaster(this.InterfaceOrientation);
		}
		
		public bool IsShowingMaster()
		{
			return (ShouldShowMaster() && _masterViewController != null && _masterViewController.View != null && _masterViewController.View.Superview == View);
		}
		
		public MGSplitViewController()
		{
			// Configure default behaviour.
			DividerView = new MGSplitDividerView(this);
			DividerView.BackgroundColor = _defaultCornerColor;
			SplitPosition = _defaultSplitPosition;
			Vertical = true;
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation interfaceOrientation)
		{
		    return true;
		}
		
		
		public override void WillRotate(UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			AdjustingMaster = true;
			
			_masterViewController.WillRotate(toInterfaceOrientation, duration);
			_detailViewController.WillRotate(toInterfaceOrientation, duration);
		}
		
		
		public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
		{
			_masterViewController.DidRotate(fromInterfaceOrientation);
			_detailViewController.DidRotate(fromInterfaceOrientation);
			
			switch (InterfaceOrientation)
			{
			case UIInterfaceOrientation.Portrait:
//				TouchFactory.NotifyOrientationChanged(iApp.Orientation.Portrait);
				break;
			case UIInterfaceOrientation.PortraitUpsideDown:
//				TouchFactory.NotifyOrientationChanged(iApp.Orientation.PortraitUpsideDown);
				break;
			case UIInterfaceOrientation.LandscapeLeft:
//				TouchFactory.NotifyOrientationChanged(iApp.Orientation.LandscapeLeft);
				break;
			case UIInterfaceOrientation.LandscapeRight:
//				TouchFactory.NotifyOrientationChanged(iApp.Orientation.LandscapeRight);
				break;
			}
		}
		
		
		public override void WillAnimateRotation(UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			_masterViewController.WillAnimateRotation(toInterfaceOrientation, duration);
			_detailViewController.WillAnimateRotation(toInterfaceOrientation, duration);
		
			// Hide popover.
			if (_hiddenPopoverController != null && _hiddenPopoverController.PopoverVisible) {
				_hiddenPopoverController.Dismiss(false);
			}
		
			// Re-tile views.
			_reconfigurePopup = true;
			LayoutSubviews(toInterfaceOrientation, true);
		}
		
		
		public override void WillAnimateFirstHalfOfRotation(UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			_masterViewController.WillAnimateFirstHalfOfRotation(toInterfaceOrientation, duration);
			_detailViewController.WillAnimateFirstHalfOfRotation(toInterfaceOrientation, duration);
		}
		
		
		public override void DidAnimateFirstHalfOfRotation(UIInterfaceOrientation toInterfaceOrientation)
		{
			_masterViewController.DidAnimateFirstHalfOfRotation(toInterfaceOrientation);
			_detailViewController.DidAnimateFirstHalfOfRotation(toInterfaceOrientation);
		}
		
		
		public override void WillAnimateSecondHalfOfRotation(UIInterfaceOrientation fromInterfaceOrientation, double duration)
		{
			_masterViewController.WillAnimateSecondHalfOfRotation(fromInterfaceOrientation, duration);
			_detailViewController.WillAnimateSecondHalfOfRotation(fromInterfaceOrientation, duration);
		}
		
		/*
		public override void PresentModalViewController (UIViewController modalViewController, bool animated)
		{
			if (ModalViewController != null)
				return;
			
			// modal form sheets have a bug where the keyboard refuses to be dismissed.
			// the available fix is only for 4.3 and greater, so we work around it by
			// using a page sheet instead and immediately resizing it after it is presented.
			// be aware that this may become broken with future versions of iOS
			modalViewController.ModalPresentationStyle = UIModalPresentationStyle.PageSheet;
			
			base.PresentModalViewController (modalViewController, animated);
			
			ModalViewController.View.Superview.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleBottomMargin;
			ModalViewController.View.Superview.Frame = new System.Drawing.RectangleF(0, 0, 540, 620);
			
			float x, y;
			if (this.IsLandscape())
			{
				x = UIScreen.MainScreen.Bounds.Height / 2;
				y = (UIScreen.MainScreen.Bounds.Width + 20) / 2;
			}
			else
			{
				x = UIScreen.MainScreen.Bounds.Width / 2;
				y = (UIScreen.MainScreen.Bounds.Height + 20) / 2;
			}
			ModalViewController.View.Superview.Center = new PointF(x, y);
		}
		
		public override void DismissModalViewControllerAnimated (bool animated)
		{
			if (ModalViewController == null)
				return;
			
			base.DismissModalViewControllerAnimated (animated);
			
			if (IsLargeFormFactor)
			{
				var controller = TouchFactory.Instance.NavControllers[1];
				if (controller.TopViewController is TableViewController)
				{
					((TableViewController)controller.TopViewController).Refresh();
				}
				else if (controller.TopViewController is MonoTouch.Dialog.DialogViewController)
				{
					((MonoTouch.Dialog.DialogViewController)controller.TopViewController).ReloadData();
				}
			}
		}
		*/
		
		SizeF SplitViewSizeForOrientation(UIInterfaceOrientation theOrientation)
		{
			UIScreen screen = UIScreen.MainScreen;
			RectangleF fullScreenRect = screen.Bounds; // always implicitly in Portrait orientation.
		
			// Find status bar height by checking which dimension of the applicationFrame is narrower than screen bounds.
			// Little bit ugly looking, but it'll still work even if they change the status bar height in future.
			//float statusBarHeight = MAX((fullScreenRect.size.width - appFrame.size.width), (fullScreenRect.size.height - appFrame.size.height));
		
			// Initially assume portrait orientation.
			float width = fullScreenRect.Size.Width;
			float height = fullScreenRect.Size.Height;
		
			// Correct for orientation.
			if (IsLandscape(theOrientation)) {
				width = height;
				height = fullScreenRect.Size.Width;
			}

			// Account for status bar, which always subtracts from the height (since it's always at the top of the screen).
			height -= 20;
		
			return new SizeF(width, height);
		}
		
		void LayoutSubviews(UIInterfaceOrientation theOrientation, bool animate)
		{
			// this is mostly for video playback.  if a video is playing, we need
			// to make sure that it remains on the top when a rotation occurs
			UIView topView = null;
			if (View.Subviews.Count() > 0)
				topView = View.Subviews.Last();
			
			if (_reconfigurePopup) {
				ReconfigureForMasterInPopover(!ShouldShowMaster(theOrientation));
			}
			
			// Layout the master, detail and divider views appropriately, adding/removing subviews as needed.
			// First obtain relevant geometry.
			SizeF fullSize = SplitViewSizeForOrientation(theOrientation);
			float width = fullSize.Width;
			float height = fullSize.Height;
			
			// Layout the master, divider and detail views.
			RectangleF newFrame = new RectangleF(0, 0, width, height);
			UIView view;
			bool shouldShowMaster = ShouldShowMaster(theOrientation);
			bool masterFirst = MasterBeforeDetail;
			RectangleF masterRect, dividerRect, detailRect;
			if (Vertical) {				
				if (masterFirst) {
					if (!ShouldShowMaster()) {
						// Move off-screen.
						newFrame.X -= (SplitPosition + SplitWidth);
					}
		
					newFrame.Width = SplitPosition;
					masterRect = newFrame;
		
					newFrame.X += newFrame.Width;
					newFrame.Width = SplitWidth;
					dividerRect = newFrame;
		
					newFrame.X += newFrame.Width;
					newFrame.Width = width - newFrame.X;
					detailRect = newFrame;
		
				} else {
					if (!ShouldShowMaster()) {
						// Move off-screen.
						newFrame.Width += (SplitPosition + SplitWidth);
					}
		
					newFrame.Width -= (SplitPosition + SplitWidth);
					detailRect = newFrame;
		
					newFrame.X += newFrame.Width;
					newFrame.Width = SplitWidth;
					dividerRect = newFrame;
		
					newFrame.X += newFrame.Width;
					newFrame.Width = SplitPosition;
					masterRect = newFrame;
				}
		
				// Position master.
				view = _masterViewController.View;
				if (view != null) {
					view.Frame = masterRect;
					if (view.Superview == null) {
						_masterViewController.ViewWillAppear(false);
						View.AddSubview(view);
						_masterViewController.ViewDidAppear(false);
					}
				}
		
				// Position divider.
				view = DividerView;
				view.Frame = dividerRect;
				if (view.Superview == null) {
					View.AddSubview(view);
				}
		
				// Position detail.
				view = _detailViewController.View;
				if (view != null) {
					view.Frame = detailRect;
					if (view.Superview == null) {
						View.InsertSubviewAbove(view, _masterViewController.View);
					} else {
						View.BringSubviewToFront(view);
					}
				}
		
			} else {
				if (masterFirst) {
					if (!ShouldShowMaster()) {
						// Move off-screen.
						newFrame.Y -= (SplitPosition + SplitWidth);
					}
		
					newFrame.Height = SplitPosition;
					masterRect = newFrame;
		
					newFrame.Y += newFrame.Height;
					newFrame.Height = SplitWidth;
					dividerRect = newFrame;
		
					newFrame.Y += newFrame.Height;
					newFrame.Height = height - newFrame.Y;
					detailRect = newFrame;
		
				} else {
					if (!ShouldShowMaster()) {
						// Move off-screen.
						newFrame.Height += (SplitPosition + SplitWidth);
					}
		
					newFrame.Height -= (SplitPosition + SplitWidth);
					detailRect = newFrame;
		
					newFrame.Y += newFrame.Height;
					newFrame.Height = SplitWidth;
					dividerRect = newFrame;
		
					newFrame.Y += newFrame.Height;
					newFrame.Height = SplitPosition;
					masterRect = newFrame;
				}
		
				// Position master.
				view = _masterViewController.View;
				if (view != null) {
					view.Frame = masterRect;
					if (view.Superview == null) {
						_masterViewController.ViewWillAppear(false);
						View.AddSubview(view);
						_masterViewController.ViewDidAppear(false);
					}
				}
		
				// Position divider.
				view = DividerView;
				view.Frame = dividerRect;
				if (view.Superview == null) {
					View.AddSubview(view);
				}
		
				// Position detail.
				view = _detailViewController.View;
				if (view == null) {
					view.Frame = detailRect;
					if (view.Superview == null) {
						View.InsertSubviewAbove(view, _masterViewController.View);
					} else {
						View.BringSubviewToFront(view);
					}
				}
			}
			
			// Create corner views if necessary.
			MGSplitCornersView leadingCorners; // top/left of screen in Vertical/horizontal split.
			MGSplitCornersView trailingCorners; // bottom/right of screen in Vertical/horizontal split.
			if (_cornerViews == null) {
				
				leadingCorners = new MGSplitCornersView();
				leadingCorners.SetSplitViewController(this);
				leadingCorners.CornerBackgroundColor = _defaultCornerColor;
				leadingCorners.CornerRadius = _defaultCornerRadius;
				trailingCorners = new MGSplitCornersView();
				trailingCorners.SetSplitViewController(this);
				trailingCorners.CornerBackgroundColor = _defaultCornerColor;
				trailingCorners.CornerRadius = _defaultCornerRadius;
				_cornerViews = new MGSplitCornersView[2]{leadingCorners, trailingCorners};
		
			} else {
				leadingCorners = _cornerViews[0];
				trailingCorners = _cornerViews[1];
			}
		
			// Configure and layout the corner-views.
			leadingCorners.CornersPosition = (Vertical) ? MGCornersPosition.LeadingVertical : MGCornersPosition.LeadingHorizontal;
			trailingCorners.CornersPosition = (Vertical) ? MGCornersPosition.TrailingVertical : MGCornersPosition.TrailingHorizontal;
			leadingCorners.AutoresizingMask = (Vertical) ? UIViewAutoresizing.FlexibleBottomMargin : UIViewAutoresizing.FlexibleRightMargin;
			trailingCorners.AutoresizingMask = (Vertical) ? UIViewAutoresizing.FlexibleTopMargin : UIViewAutoresizing.FlexibleLeftMargin;
		
			float x, y, cornersWidth, cornersHeight;
			RectangleF leadingRect, trailingRect;
			float radius = leadingCorners.CornerRadius;
			if (Vertical) { // left/right split
				cornersWidth = (radius * 2.0f) + SplitWidth;
				cornersHeight = radius;
				x = ((shouldShowMaster) ? ((masterFirst) ? SplitPosition : width - (SplitPosition + SplitWidth)) : (0 - SplitWidth)) - radius;
				y = 0;
				leadingRect = new RectangleF(x, y, cornersWidth, cornersHeight); // top corners
				trailingRect = new RectangleF(x, (height - cornersHeight), cornersWidth, cornersHeight); // bottom corners
		
			} else { // top/bottom split
				x = 0;
				y = ((shouldShowMaster) ? ((masterFirst) ? SplitPosition : height - (SplitPosition + SplitWidth)) : (0 - SplitWidth)) - radius;
				cornersWidth = radius;
				cornersHeight = (radius * 2.0f) + SplitWidth;
				leadingRect = new RectangleF(x, y, cornersWidth, cornersHeight); // left corners
				trailingRect = new RectangleF((width - cornersWidth), y, cornersWidth, cornersHeight); // right corners
			}
		
			leadingCorners.Frame = leadingRect;
			trailingCorners.Frame = trailingRect;
		
			// Ensure corners are visible and frontmost.
			if (leadingCorners.Superview == null) {
				View.InsertSubviewAbove(leadingCorners, _detailViewController.View);
				View.InsertSubviewAbove(trailingCorners, _detailViewController.View);
			} else {
				View.BringSubviewToFront(leadingCorners);
				View.BringSubviewToFront(trailingCorners);
			}
			
			if (topView != null)
			{
				topView.Superview.BringSubviewToFront(topView);
			}

//			if (IsLandscape())
//				View.Frame = new RectangleF(0, 20, View.Frame.Width, View.Frame.Height);
//			else
//				View.Frame = new RectangleF(0, 20, View.Frame.Width, View.Frame.Height);
		}
		
		void LayoutSubviews(bool animate)
		{
			LayoutSubviews(this.InterfaceOrientation, animate);
		}
		
		
		internal void LayoutSubviews()
		{
			LayoutSubviews(this.InterfaceOrientation, true);
		}
		
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			
			if (IsShowingMaster()) {
				_masterViewController.ViewWillAppear(animated);
			}
			_detailViewController.ViewWillAppear(animated);
		
			_reconfigurePopup = true;
			LayoutSubviews();
		}
		
		
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			
			if (IsShowingMaster()) {
				_masterViewController.ViewDidAppear(animated);
			}
			_detailViewController.ViewDidAppear(animated);
		}
		
		
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		
			if (IsShowingMaster()) {
				_masterViewController.ViewWillDisappear(animated);
			}
			_detailViewController.ViewWillDisappear(animated);
		}
		
		
		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		
			if (IsShowingMaster()) {
				_masterViewController.ViewDidDisappear(animated);
			}
			_detailViewController.ViewDidDisappear(animated);
		}
		
		public void UpdateMasterButton()
		{
			UINavigationController detailViewController = _detailViewController as UINavigationController;
			
			if (_barButtonItem != null)
			{
				_barButtonItem.Title = _barButtonItemText;
				detailViewController.VisibleViewController.NavigationItem.SetLeftBarButtonItem(_barButtonItem, false);
			}
		}
		
		void ReconfigureForMasterInPopover(bool inPopover)
		{
			if (_masterViewController == null)
				return;
			
			AdjustingMaster = true;
			UINavigationController detailNavigationController = _detailViewController as UINavigationController;
			_reconfigurePopup = false;
		
			if ((inPopover && _hiddenPopoverController != null) || (!inPopover && _hiddenPopoverController == null)) {
				// Nothing to do.
				return;
			}
		
			if (inPopover && _hiddenPopoverController == null && _barButtonItem == null)
			{
				// Create and configure popover for our masterViewController.
				_masterViewController.ViewWillDisappear(false);
				_hiddenPopoverController = new UIPopoverController(_masterViewController);
				_hiddenPopoverController.DidDismiss += delegate {
					AdjustingMaster = true;
				};
				_masterViewController.ViewDidDisappear(false);
				
				// Create and configure _barButtonItem.

			    _barButtonItem = new UIBarButtonItem(_barButtonItemText, UIBarButtonItemStyle.Bordered, delegate { ShowMasterPopover(); });
				detailNavigationController.VisibleViewController.NavigationItem.SetLeftBarButtonItem(_barButtonItem, true);
				// Inform delegate of this state of affairs.
				if (_splitViewDelegate != null) {
					_splitViewDelegate.WillHideViewController(_masterViewController, _barButtonItem, _hiddenPopoverController);
				}
			}
			else if (!inPopover && _hiddenPopoverController != null && _barButtonItem != null)
			{
				// I know this looks strange, but it fixes a bizarre issue with UIPopoverController leaving masterViewController's views in disarray.
				_hiddenPopoverController.PresentFromRect(RectangleF.Empty, View, UIPopoverArrowDirection.Any, false);
		
				// Remove master from popover and destroy popover, if it exists.
				_hiddenPopoverController.Dismiss(false);
				_hiddenPopoverController = null;
		
				// Inform delegate that the _barButtonItem will become invalid.
				if (_splitViewDelegate != null) {
					_splitViewDelegate.WillShowViewController(_masterViewController, _barButtonItem);
				}
				// Destroy _barButtonItem.
				detailNavigationController.VisibleViewController.NavigationItem.LeftBarButtonItem = null;
				_barButtonItem = null;
		
				// Move master view.
				UIView masterView = _masterViewController.View;
				if (masterView != null && masterView.Superview != View) {
					masterView.RemoveFromSuperview();
				}
			}
		}
		
		
		void PopoverControllerDidDismissPopover(UIPopoverController popoverController)
		{
			ReconfigureForMasterInPopover(false);
		}
		
		
		void NotePopoverDismissed()
		{
			PopoverControllerDidDismissPopover(_hiddenPopoverController);
		}
		
		
		void AnimationDidStop(string animationID, int finished)
		{
			if ((animationID == _animationChangeSplitOrientation || 
				 animationID == _animationChangeSubviewsOrder) && _cornerViews != null) {
				foreach (UIView corner in _cornerViews) {
					corner.Hidden = false;
				}
				DividerView.Hidden = false;
			}
		}
		

		public void ToggleSplitOrientation()
		{
			bool showingMaster = this.IsShowingMaster();
			if (showingMaster) {
				if (_cornerViews != null) {
					foreach (UIView corner in _cornerViews) {
						corner.Hidden = true;
					}
					DividerView.Hidden = true;
				}
				UIView.BeginAnimations(_animationChangeSplitOrientation);
				UIView.SetAnimationDelegate(this);
				UIView.SetAnimationDidStopSelector(new Selector("AnimationDidStop(finished, context)"));
			}
			Vertical = !Vertical;
			if (showingMaster) {
				UIView.CommitAnimations();
			}
		}
		
		
		public void ToggleMasterBeforeDetail()
		{
			bool showingMaster = IsShowingMaster();
			if (showingMaster) {
				if (_cornerViews != null) {
					foreach (UIView corner in _cornerViews) {
						corner.Hidden = true;
					}
					DividerView.Hidden = true;
				}
				UIView.BeginAnimations(_animationChangeSubviewsOrder);
				UIView.SetAnimationDelegate(this);
				UIView.SetAnimationDidStopSelector(new Selector("AnimationDidStop(finished, context)"));
			}
			MasterBeforeDetail = !MasterBeforeDetail;
			if (showingMaster) {
				UIView.CommitAnimations();
			}
		}
		
		
		public void HidePopover()
		{
			if (_hiddenPopoverController != null && _hiddenPopoverController.PopoverVisible)
			{
				_hiddenPopoverController.Dismiss(true);
				_reconfigurePopup = true;
				LayoutSubviews();
			}
		}
		
		
		public void ToggleMasterView()
		{
			if (_hiddenPopoverController != null && _hiddenPopoverController.PopoverVisible) {
				_hiddenPopoverController.Dismiss(false);
			}
		
			if (!IsShowingMaster()) {
				// We're about to show the master view. Ensure it's in place off-screen to be animated in.
				_reconfigurePopup = true;
				ReconfigureForMasterInPopover(false);
				LayoutSubviews();
			}
		
			// This action functions on the current primary orientation; it is independent of the other primary orientation.
			UIView.BeginAnimations("toggleMaster");
			if (IsLandscape(InterfaceOrientation)) {
				_showsMasterInLandscape = !_showsMasterInLandscape;
			} else {
				_showsMasterInPortrait = !_showsMasterInPortrait;
			}
			UIView.CommitAnimations();
		}
		
		
		public void ShowMasterPopover()
		{
			if (_hiddenPopoverController != null && !(_hiddenPopoverController.PopoverVisible)) {
				// Inform delegate.
				if (_splitViewDelegate != null) {
					_splitViewDelegate.WillPresentViewController(_masterViewController);
				}
		
				// Show popover.
				_hiddenPopoverController.PresentFromBarButtonItem(_barButtonItem, UIPopoverArrowDirection.Any, true);
			}
		}
		
		public void SetDelegate(MGSplitViewControllerDelegate newDelegate)
		{
			if (newDelegate != _splitViewDelegate) {
				_splitViewDelegate = newDelegate;
			}
		}
		
		
		public void SetShowsMasterInPortrait(bool flag)
		{
			if (flag != _showsMasterInPortrait) {
				_showsMasterInPortrait = flag;
		
//				if (IsLandscape(InterfaceOrientation)) { // i.e. if this will cause a visual change.
					if (_hiddenPopoverController != null && _hiddenPopoverController.PopoverVisible) {
						_hiddenPopoverController.Dismiss(false);
					}
		
					// Rearrange views.
					_reconfigurePopup = true;
					LayoutSubviews();
//				}
			}
		}
		
		
		public void SetShowsMasterInLandscape(bool flag)
		{
			if (flag != _showsMasterInLandscape) {
				_showsMasterInLandscape = flag;
		
				if (IsLandscape(InterfaceOrientation)) { // i.e. if this will cause a visual change.
					if (_hiddenPopoverController != null && _hiddenPopoverController.PopoverVisible) {
						_hiddenPopoverController.Dismiss(false);
					}
		
					// Rearrange views.
					_reconfigurePopup = true;
					LayoutSubviews();
				}
			}
		}
		
		
		public void SetVertical(bool flag)
		{
			if (flag != Vertical) {
				if (_hiddenPopoverController != null && _hiddenPopoverController.PopoverVisible) {
					_hiddenPopoverController.Dismiss(false);
				}
		
				Vertical = flag;
		
				// Inform delegate.
				if (_splitViewDelegate != null) {
					_splitViewDelegate.WillChangeSplitOrientationToVertical(Vertical);
				}
		
				LayoutSubviews();
			}
		}
		
		
		public void SetMasterBeforeDetail(bool flag)
		{
			if (flag != MasterBeforeDetail) {
				if (_hiddenPopoverController != null && _hiddenPopoverController.PopoverVisible) {
					_hiddenPopoverController.Dismiss(false);
				}
		
				MasterBeforeDetail = flag;
		
				if (IsShowingMaster()) {
					LayoutSubviews();
				}
			}
		}
		
		
		public void SetSplitPosition(float pos)
		{
			// Check to see if delegate wishes to constrain the position.
			float newPos = pos;
			bool constrained = false;
			SizeF fullSize = SplitViewSizeForOrientation(InterfaceOrientation);
			if (_splitViewDelegate != null)
			{
				newPos = _splitViewDelegate.ConstrainSplitPosition(newPos, fullSize);
				constrained = true; // implicitly trust delegate's response.
			}
			else
			{
				// Apply default constraints if delegate doesn't wish to participate.
				float minPos = _minViewWidth;
				float maxPos = ((Vertical) ? fullSize.Width : fullSize.Height) - (_minViewWidth + SplitWidth);
				constrained = (newPos != SplitPosition && newPos >= minPos && newPos <= maxPos);
			}
		
			if (constrained)
			{
				if (_hiddenPopoverController != null && _hiddenPopoverController.PopoverVisible)
					_hiddenPopoverController.Dismiss(false);
		
				SplitPosition = newPos;
		
				// Inform delegate.
				if (_splitViewDelegate != null)
					_splitViewDelegate.WillMoveSplitToPosition(SplitPosition);
		
				if (IsShowingMaster())
					LayoutSubviews();
			}
		}
		
		
		public void SetSplitPosition(float pos, bool animate)
		{
			bool shouldAnimate = (animate && IsShowingMaster());
			if (shouldAnimate) {
				UIView.BeginAnimations("SplitPosition");
			}
			SetSplitPosition(pos);
			if (shouldAnimate) {
				UIView.CommitAnimations();
			}
		}
		
		
		public void SetSplitWidth(float width)
		{
			if (width != SplitWidth && width >= 0) {
				SplitWidth = width;
				if (IsShowingMaster()) {
					LayoutSubviews();
				}
			}
		}
		
		
		public void SetViewControllers(UIViewController[] controllers)
		{
			System.Diagnostics.Debug.Assert(controllers != null);
			System.Diagnostics.Debug.Assert(controllers.Length == 2);

			if (_masterViewController != null)
				_masterViewController.View.RemoveFromSuperview();
			if (_detailViewController != null)
				_detailViewController.View.RemoveFromSuperview();
			
			_masterViewController = controllers[0];
			_detailViewController = controllers[1];

			LayoutSubviews();
		}
		
		
		public void SetMasterViewController(UIViewController master)
		{
			if (_masterViewController != master) {
				_masterViewController = master;
				LayoutSubviews();
			}
		}
		
		
		public void SetDetailViewController(UIViewController detail)
		{
			if (_detailViewController != detail) {
				_detailViewController = detail;
				LayoutSubviews();
			}
		}
		
		
		void SetDividerView(MGSplitDividerView divider)
		{
			if (divider != DividerView) {
				DividerView.RemoveFromSuperview();

				DividerView = divider;
				DividerView.SplitViewController = this;
				DividerView.BackgroundColor = _defaultCornerColor;
				if (IsShowingMaster()) {
					LayoutSubviews();
				}
			}
		}
		
		
		public bool AllowsDraggingDivider()
		{
			if (DividerView != null) {
				return DividerView.AllowsDragging;
			}
		
			return false;
		}
		
		
		private void SetAllowsDraggingDivider(bool flag)
		{
			if (this.AllowsDraggingDivider() != flag && DividerView != null) {
				DividerView.AllowsDragging = flag;
			}
		}
		
		
		public void SetDividerStyle(MGSplitViewDividerStyle newStyle)
		{
			if (_hiddenPopoverController != null && _hiddenPopoverController.PopoverVisible) {
				_hiddenPopoverController.Dismiss(false);
			}
		
			// We don't check to see if newStyle equals _dividerStyle, because it's a meta-setting.
			// Aspects could have been changed since it was set.
			DividerStyle = newStyle;
		
			// Reconfigure general appearance and behaviour.
			float cornerRadius = 0;
			if (DividerStyle == MGSplitViewDividerStyle.Thin) {
				cornerRadius = _defaultCornerRadius;
				SplitWidth = DefaultThinWidth;
				SetAllowsDraggingDivider(false);
		
			} else if (DividerStyle == MGSplitViewDividerStyle.PaneSplitter) {
				cornerRadius = _panesplitterCornerRadius;
				SplitWidth = DefaultThickWidth;
				SetAllowsDraggingDivider(true);
			}
		
			// Update divider and corners.
			DividerView.SetNeedsDisplay();
			if (_cornerViews != null) {
				foreach (MGSplitCornersView corner in _cornerViews) {
					corner.CornerRadius = cornerRadius;
				}
			}
		
			// Layout all views.
			LayoutSubviews();
		}
		
		
		public void SetDividerStyle(MGSplitViewDividerStyle newStyle, bool animate)
		{
			bool shouldAnimate = (animate && IsShowingMaster());
			if (shouldAnimate) {
				UIView.BeginAnimations("DividerStyle");
			}
			SetDividerStyle(newStyle);
			if (shouldAnimate) {
				UIView.CommitAnimations();
			}
		}		
	}
}
