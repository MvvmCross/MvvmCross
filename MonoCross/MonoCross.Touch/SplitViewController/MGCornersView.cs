using System;
using System.Drawing;

using MonoTouch.CoreGraphics;
using MonoTouch.UIKit;

namespace MonoCross.Touch
{
	internal enum MGCornersPosition
	{
		LeadingVertical, // top of screen for a left/right split.
		TrailingVertical, // bottom of screen for a left/right split.
		LeadingHorizontal, // left of screen for a top/bottom split.
		TrailingHorizontal // right of screen for a top/bottom split.
	}
	
	class MGSplitCornersView : UIView
	{
		internal float CornerRadius { get; set; }
		internal MGCornersPosition CornersPosition { get; set; }
		internal UIColor CornerBackgroundColor { get; set; }
		
		private MGSplitViewController _splitViewController = null;
		
		internal MGSplitCornersView ()
		{
			ContentMode = UIViewContentMode.Redraw;
			UserInteractionEnabled = false;
			Opaque = false;
			BackgroundColor = UIColor.Clear;
			CornerRadius = 0; // actual value is set by the splitViewController.
			CornersPosition = MGCornersPosition.LeadingVertical;
		}
		
		
		float DegreesToRadians(float degrees)
		{
			// Converts degrees to radians.
			return degrees * ((float)Math.PI / 180);
		}
		
		
		float RadiansToDegrees(float radians)
		{
			// Converts radians to degrees.
			return radians * (180 / (float)Math.PI);
		}
		
		
		void DrawRect(RectangleF rect)
		{
			// Draw two appropriate corners, with cornerBackgroundColor behind them.
			if (CornerRadius > 0) {
		
				float maxX = Bounds.GetMaxX();
				float maxY = Bounds.GetMaxY();
				UIBezierPath path = UIBezierPath.Create();
				PointF pt = PointF.Empty;
				switch (CornersPosition) {
					case MGCornersPosition.LeadingVertical: // top of screen for a left/right split
						path.MoveTo(pt);
						pt.Y += CornerRadius;
						path.AppendPath(UIBezierPath.FromArc(pt, CornerRadius, DegreesToRadians(90), 0, true));
						pt.X += CornerRadius;
						pt.Y -= CornerRadius;
						path.AddLineTo(pt);
						path.AddLineTo(PointF.Empty);
						path.ClosePath();
		
						pt.X = maxX - CornerRadius;
						pt.Y = 0;
						path.MoveTo(pt);
						pt.Y = maxY;
						path.AddLineTo(pt);
						pt.X += CornerRadius;
						path.AppendPath(UIBezierPath.FromArc(pt, CornerRadius, DegreesToRadians(180), DegreesToRadians(90), true));
						pt.Y -= CornerRadius;
						path.AddLineTo(pt);
						pt.X -= CornerRadius;
						path.AddLineTo(pt);
						path.ClosePath();
		
						break;
		
					case MGCornersPosition.TrailingVertical: // bottom of screen for a left/right split
						pt.Y = maxY;			
						path.MoveTo(pt);
						pt.Y -= CornerRadius;
						path.AppendPath(UIBezierPath.FromArc(pt, CornerRadius, DegreesToRadians(270), DegreesToRadians(360), false));
						pt.X += CornerRadius;
						pt.Y += CornerRadius;
						path.AddLineTo(pt);
						pt.X -= CornerRadius;
						path.AddLineTo(pt);
						path.ClosePath();
		
						pt.X = maxX - CornerRadius;
						pt.Y = maxY;
						path.MoveTo(pt);
						pt.Y -= CornerRadius;
						path.AddLineTo(pt);
						pt.X += CornerRadius;
						path.AppendPath(UIBezierPath.FromArc(pt, CornerRadius, DegreesToRadians(180), DegreesToRadians(270), false));
						pt.Y += CornerRadius;
						path.AddLineTo(pt);
						pt.X -= CornerRadius;
						path.AddLineTo(pt);
						path.ClosePath();
		
						break;
		
					case MGCornersPosition.LeadingHorizontal: // left of screen for a top/bottom split
						pt.X = 0;
						pt.Y = CornerRadius;
						path.MoveTo(pt);
						pt.Y -= CornerRadius;
						path.AddLineTo(pt);
						pt.X += CornerRadius;
						path.AppendPath(UIBezierPath.FromArc(pt, CornerRadius, DegreesToRadians(180), DegreesToRadians(270), false));
						pt.Y += CornerRadius;
						path.AddLineTo(pt);
						pt.X -= CornerRadius;
						path.AddLineTo(pt);
						path.ClosePath();
		
						pt.X = 0;
						pt.Y = maxY - CornerRadius;
						path.MoveTo(pt);
						pt.Y = maxY;
						path.AddLineTo(pt);
						pt.X += CornerRadius;
						path.AppendPath(UIBezierPath.FromArc(pt, CornerRadius, DegreesToRadians(180), DegreesToRadians(90), true));
						pt.Y -= CornerRadius;
						path.AddLineTo(pt);
						pt.X -= CornerRadius;
						path.AddLineTo(pt);
						path.ClosePath();
		
						break;
		
					case MGCornersPosition.TrailingHorizontal: // right of screen for a top/bottom split
						pt.Y = CornerRadius;
						path.MoveTo(pt);
						pt.Y -= CornerRadius;
						path.AppendPath(UIBezierPath.FromArc(pt, CornerRadius, DegreesToRadians(270), DegreesToRadians(360), false));
						pt.X += CornerRadius;
						pt.Y += CornerRadius;
						path.AddLineTo(pt);
						pt.X -= CornerRadius;
						path.AddLineTo(pt);
						path.ClosePath();
		
						pt.Y = maxY - CornerRadius;
						path.MoveTo(pt);
						pt.Y += CornerRadius;
						path.AppendPath(UIBezierPath.FromArc(pt, CornerRadius, DegreesToRadians(90), 0, true));
						pt.X += CornerRadius;
						pt.Y -= CornerRadius;
						path.AddLineTo(pt);
						pt.X -= CornerRadius;
						path.AddLineTo(pt);
						path.ClosePath();
					
						break;
		
					default:
						break;
				}
		
				CornerBackgroundColor.SetColor();
				path.Fill();
			}
		}
		
		
		public void SetCornerRadius(float newRadius)
		{
			if (newRadius != CornerRadius) {
				CornerRadius = newRadius;
				SetNeedsDisplay();
			}
		}
		
		public void SetSplitViewController(MGSplitViewController controller)
		{
			if (controller != _splitViewController) {
				_splitViewController = controller;
				SetNeedsDisplay();
			}
		}
		
		
		public void SetCornersPosition(MGCornersPosition pos)
		{
			if (CornersPosition != pos) {
				CornersPosition = pos;
				SetNeedsDisplay();
			}
		}
		
		
		public void SetCornerBackgroundColor(UIColor color)
		{
			if (color != CornerBackgroundColor) {
				CornerBackgroundColor = color;
				SetNeedsDisplay();
			}
		}
	}
}

