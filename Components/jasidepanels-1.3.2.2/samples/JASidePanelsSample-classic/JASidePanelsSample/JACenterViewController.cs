using System;
#if __UNIFIED__
using UIKit;
#else
using MonoTouch.UIKit;
#endif

using JASidePanels;

namespace JASidePanelsSample
{
	public class JACenterViewController : JADebugViewController
	{
		public JACenterViewController ()
		{
			Title = "Center Panel";
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Random rnd = new Random ();

			View.BackgroundColor = UIColor.FromRGB (
				(float)rnd.NextDouble (),
				(float)rnd.NextDouble (),
				(float)rnd.NextDouble ());
		}
	}
}
