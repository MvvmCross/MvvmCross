using System;
using System.Drawing;

using MonoTouch.CoreGraphics;
using MonoTouch.UIKit;

namespace MonoCross.Touch
{
	class MGSplitDividerView : UIView
	{
		internal MGSplitViewController SplitViewController { get; set; }
		internal bool AllowsDragging
		{
			get { return UserInteractionEnabled; }
			set
			{ 
				UserInteractionEnabled = value;
				
				SplitViewController.SplitWidth = value ?
					SplitViewController.DefaultThickWidth : SplitViewController.DefaultThinWidth;
				
				SplitViewController.DividerStyle = value ?
					MGSplitViewDividerStyle.PaneSplitter : MGSplitViewDividerStyle.Thin;
			}
		}
		
		public MGSplitDividerView(MGSplitViewController splitViewController)
		{
			SplitViewController = splitViewController;
			
// TODO: set up options for this!!!
//			AllowsDragging = TouchFactory.Instance.AllowSplitViewResizing;
//
			ContentMode = UIViewContentMode.Redraw;
		}
		
		public override void Draw (RectangleF rect)
		{
			if (SplitViewController.DividerStyle == MGSplitViewDividerStyle.Thin) {
				base.Draw(rect);
			
			} else if (SplitViewController.DividerStyle == MGSplitViewDividerStyle.PaneSplitter) {
				// Draw gradient background.
				RectangleF bounds = Bounds;
				CGColorSpace rgb = CGColorSpace.CreateDeviceRGB();
				float[] locations = {0, 1};
				float[] components = {	0.988f, 0.988f, 0.988f, 1.0f,  // light
										0.875f, 0.875f, 0.875f, 1.0f };// dark
				CGGradient gradient = new CGGradient(rgb, components, locations);
				CGContext context = UIGraphics.GetCurrentContext();
				PointF start, end;
				if (SplitViewController.Vertical) {
					// Light left to dark right.
					start = new PointF(bounds.GetMinX(), bounds.GetMidY());
					end = new PointF(bounds.GetMaxX(), bounds.GetMidY());
				} else {
					// Light top to dark bottom.
					start = new PointF(bounds.GetMidX(), bounds.GetMinY());
					end = new PointF(bounds.GetMidX(), bounds.GetMaxY());
				}
				context.DrawLinearGradient(gradient, start, end, CGGradientDrawingOptions.DrawsBeforeStartLocation);
		
				// Draw borders.
				float borderThickness = 10;
				UIColor.FromWhiteAlpha(0.7f, 1).SetColor();
				RectangleF borderRect = bounds;
				if (SplitViewController.Vertical) {
					borderRect.Width = borderThickness;
					context.FillRect(borderRect);
					borderRect.X = bounds.GetMaxX() - borderThickness;
					context.FillRect(borderRect);
		
				} else {
					borderRect.Height = borderThickness;
					context.FillRect(borderRect);
					borderRect.Y = bounds.GetMaxY() - borderThickness;
					context.FillRect(borderRect);
				}

				// Draw grip.
				DrawGripThumbInRect(bounds);
				
			}
		}
		
		
		void DrawGripThumbInRect(RectangleF rect)
		{
			float width = SplitViewController.Vertical ? 9 : 30;
			float height = SplitViewController.Vertical ? 30 : 9;
		
			// Draw grip in centred in rect.
			RectangleF gripRect = new RectangleF(0, 0, width, height);
			gripRect.X = ((rect.Width - gripRect.Width) / 2.0f);
			gripRect.Y = ((rect.Height - gripRect.Height) / 2.0f);
		
			float stripThickness = 10;
			UIColor stripColor = UIColor.FromWhiteAlpha(0.35f, 1);
			UIColor lightColor = UIColor.FromWhiteAlpha(1, 1);
			CGContext context = UIGraphics.GetCurrentContext();
			float space = 3;
			if (SplitViewController.Vertical) {
				gripRect.Width = stripThickness;
				stripColor.SetColor();
				context.FillRect(gripRect);
		
				gripRect.X += stripThickness;
				gripRect.Y += 1;
				lightColor.SetColor();
				context.FillRect(gripRect);
				gripRect.X -= stripThickness;
				gripRect.Y -= 1;
		
				gripRect.X += space + stripThickness;
				stripColor.SetColor();
				context.FillRect(gripRect);
		
				gripRect.X += stripThickness;
				gripRect.Y += 1;
				lightColor.SetColor();
				context.FillRect(gripRect);
				gripRect.X -= stripThickness;
				gripRect.Y -= 1;
		
				gripRect.X += space + stripThickness;
				stripColor.SetColor();
				context.FillRect(gripRect);
		
				gripRect.X += stripThickness;
				gripRect.Y += 1;
				lightColor.SetColor();
				context.FillRect(gripRect);
		
			} else {
				gripRect.Height = stripThickness;
				stripColor.SetColor();
				context.FillRect(gripRect);
		
				gripRect.Y += stripThickness;
				gripRect.X -= 1;
				lightColor.SetColor();
				context.FillRect(gripRect);
				gripRect.Y -= stripThickness;
				gripRect.X += 1;
		
				gripRect.Y += space + stripThickness;
				stripColor.SetColor();
				context.FillRect(gripRect);
		
				gripRect.Y += stripThickness;
				gripRect.X -= 1;
				lightColor.SetColor();
				context.FillRect(gripRect);
				gripRect.Y -= stripThickness;
				gripRect.X += 1;
		
				gripRect.Y += space + stripThickness;
				stripColor.SetColor();
				context.FillRect(gripRect);
		
				gripRect.Y += stripThickness;
				gripRect.X -= 1;
				lightColor.SetColor();
				
				context.FillRect(gripRect);
			}
		}
		
		
		public override void TouchesMoved (MonoTouch.Foundation.NSSet touches, UIEvent evt)
		{
			UITouch touch = (UITouch)touches.AnyObject;
			if (touch != null) {
				PointF lastPt = touch.PreviousLocationInView(this);
				PointF pt = touch.LocationInView(this);
				float offset = (SplitViewController.Vertical) ? pt.X - lastPt.X : pt.Y - lastPt.Y;
				if (!SplitViewController.MasterBeforeDetail) {
					offset = -offset;
				}
				SplitViewController.SplitPosition = SplitViewController.SplitPosition + offset;
				SplitViewController.LayoutSubviews();
				
			}
		}
	}
}

