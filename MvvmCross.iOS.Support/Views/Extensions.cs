namespace MvvmCross.iOS.Support.Views
{
    using System;
    using System.Linq;
    using CoreGraphics;
    using Foundation;
    using UIKit;

    public static class ViewExtensions
	{
		/// <summary>
		/// Find the first responder in the <paramref name="view"/>'s subview hierarchy
		/// </summary>
		/// <param name="view">
		/// A <see cref="UIView"/>
		/// </param>
		/// <returns>
		/// A <see cref="UIView"/> that is the first responder or null if there is no first responder
		/// </returns>
		public static UIView FindFirstResponder(this UIView view)
		{
			if (view.IsFirstResponder)
			{
				return view;
			}

			return view.Subviews.Select(subView => subView.FindFirstResponder()).FirstOrDefault(firstResponder => firstResponder != null);
		}

		/// <summary>
		/// Find the first Superview of the specified type (or descendant of)
		/// </summary>
		/// <param name="view">
		/// A <see cref="UIView"/>
		/// </param>
		/// <param name="stopAt">
		/// A <see cref="UIView"/> that indicates where to stop looking up the superview hierarchy
		/// </param>
		/// <param name="type">
		/// A <see cref="Type"/> to look for, this should be a UIView or descendant type
		/// </param>
		/// <returns>
		/// A <see cref="UIView"/> if it is found, otherwise null
		/// </returns>
		public static UIView FindSuperviewOfType(this UIView view, UIView stopAt, Type type)
		{
			if (view.Superview != null)
			{
				if (type.IsInstanceOfType(view.Superview))
				{
					return view.Superview;
				}

				if (!Equals(view.Superview, stopAt))
				{
					return view.Superview.FindSuperviewOfType(stopAt, type);
				}
			}

			return null;
		}

		public static UIView FindTopSuperviewOfType(this UIView view, UIView stopAt, Type type)
		{
			var superview = view.FindSuperviewOfType(stopAt, type);
			var topSuperView = superview;
			while (superview != null && !Equals(superview, stopAt))
			{
				superview = superview.FindSuperviewOfType(stopAt, type);
				if (superview != null)
				{
					topSuperView = superview;
				}
			}

			return topSuperView;
		}

		public static UIMotionEffect SetParallaxIntensity(this UIView view, float parallaxDepth, float? verticalDepth = null)
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(7, 0))
			{
				float vertical = verticalDepth ?? parallaxDepth;
				var verticalMotionEffect = new UIInterpolatingMotionEffect("center.y", UIInterpolatingMotionEffectType.TiltAlongVerticalAxis);
				verticalMotionEffect.MinimumRelativeValue = new NSNumber(-vertical);
				verticalMotionEffect.MaximumRelativeValue = new NSNumber(vertical);
				var horizontalMotionEffect = new UIInterpolatingMotionEffect("center.x", UIInterpolatingMotionEffectType.TiltAlongHorizontalAxis);
				horizontalMotionEffect.MinimumRelativeValue = new NSNumber(-parallaxDepth);
				horizontalMotionEffect.MaximumRelativeValue = new NSNumber(parallaxDepth);
				var group = new UIMotionEffectGroup();
				group.MotionEffects = new UIMotionEffect[] { horizontalMotionEffect, verticalMotionEffect };
				view.AddMotionEffect(group);
				return group;
			}

			return null;
		}

		public static void CenterView(this UIScrollView scrollView, UIView viewToCenter, CGRect keyboardFrame, bool animated = false)
		{
			var scrollFrame = scrollView.Frame;

			var adjustedFrame = UIApplication.SharedApplication.KeyWindow.ConvertRectFromView(scrollFrame, scrollView.Superview);

			var intersect = CGRect.Intersect(adjustedFrame, keyboardFrame);

			var height = intersect.Height;
			if (!UIDevice.CurrentDevice.CheckSystemVersion(8, 0) && IsLandscape())
			{
				height = intersect.Width;
			}

			scrollView.CenterView(viewToCenter, height, animated: animated);
		}

		public static void CenterView(this UIScrollView scrollView, UIView viewToCenter, nfloat keyboardHeight = default(nfloat), bool adjustContentInsets = true, bool animated = false)
		{
			if (adjustContentInsets)
			{
				var contentInsets = new UIEdgeInsets(0.0f, 0.0f, keyboardHeight, 0.0f);
				scrollView.ContentInset = contentInsets;
				scrollView.ScrollIndicatorInsets = contentInsets;
			}

			// Position of the active field relative isnside the scroll view
			CGRect relativeFrame = viewToCenter.Superview.ConvertRectToView(viewToCenter.Frame, scrollView);

			var spaceAboveKeyboard = scrollView.Frame.Height - keyboardHeight;

			// Move the active field to the center of the available space
			var offset = relativeFrame.Y - (spaceAboveKeyboard - viewToCenter.Frame.Height) / 2;
			if (scrollView.ContentOffset.Y < offset)
			{
				scrollView.SetContentOffset(new CGPoint(0, offset), animated);
			}
		}

		public static void RestoreScrollPosition(this UIScrollView scrollView)
		{
			scrollView.ContentInset = UIEdgeInsets.Zero;
			scrollView.ScrollIndicatorInsets = UIEdgeInsets.Zero;
		}

		public static bool IsLandscape()
		{
			var orientation = UIApplication.SharedApplication.StatusBarOrientation;
			bool landscape = orientation == UIInterfaceOrientation.LandscapeLeft || orientation == UIInterfaceOrientation.LandscapeRight;
			return landscape;
		}
	}
}

