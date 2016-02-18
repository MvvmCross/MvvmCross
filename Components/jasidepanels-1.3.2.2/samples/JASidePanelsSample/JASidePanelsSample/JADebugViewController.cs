using System;
#if __UNIFIED__
using UIKit;
#else
using MonoTouch.UIKit;
#endif

using JASidePanels;

namespace JASidePanelsSample
{
	public class JADebugViewController : UIViewController
	{
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			Console.WriteLine ("{0}.ViewWillAppear", this);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			Console.WriteLine ("{0}.ViewDidAppear", this);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			Console.WriteLine ("{0}.ViewWillDisappear", this);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);

			Console.WriteLine ("{0}.ViewDidDisappear", this);
		}

		public override void WillMoveToParentViewController (UIViewController parent)
		{
			base.WillMoveToParentViewController (parent);

			Console.WriteLine ("{0}.WillMoveToParentViewController ({1})", this, parent);
		}

		public override void DidMoveToParentViewController (UIViewController parent)
		{
			base.DidMoveToParentViewController (parent);

			Console.WriteLine ("{0}.WillMoveToParentViewController ({1})", this, parent);
		}
	}
}

