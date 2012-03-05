// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Tutorial.UI.Touch.Views
{
	[Register ("TipView")]
	partial class TipView
	{
		[Outlet]
		MonoTouch.UIKit.UILabel TipValueLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel TotalLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UISlider TipPercentSlider { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField TipPercentText { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField SubTotalText { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (TipValueLabel != null) {
				TipValueLabel.Dispose ();
				TipValueLabel = null;
			}

			if (TotalLabel != null) {
				TotalLabel.Dispose ();
				TotalLabel = null;
			}

			if (TipPercentSlider != null) {
				TipPercentSlider.Dispose ();
				TipPercentSlider = null;
			}

			if (TipPercentText != null) {
				TipPercentText.Dispose ();
				TipPercentText = null;
			}

			if (SubTotalText != null) {
				SubTotalText.Dispose ();
				SubTotalText = null;
			}
		}
	}
}
