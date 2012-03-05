using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Tutorial.Core.ViewModels.Lessons;
using Cirrious.MvvmCross.Binding.Touch.Views;
using System.Collections.Generic;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Binding.Touch.ExtensionMethods;

namespace Tutorial.UI.Touch.Views
{
    public partial class TipView : MvxBindingTouchViewController<TipViewModel>
    {
        public TipView (MvxShowViewModelRequest request) : base (request, "TipView", null)
        {
        }
		
        public override void DidReceiveMemoryWarning ()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning ();
			
            // Release any cached data, images, etc that aren't in use.
        }
		
        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();


            // Perform any additional setup after loading the view, typically from a nib.
            this.AddBindings(
                new Dictionary<object, string>()
                    {
                        { TipValueLabel, "{'Text':{'Path':'TipValue'}}" },
                        { TotalLabel, "{'Text':{'Path':'Total'}}" },
                        { TipPercentText, "{'Text':{'Path':'TipPercent','Converter':'Int','Mode':'TwoWay'}}" },
                        { TipPercentSlider, "{'Value':{'Path':'TipPercent','Converter':'IntToFloat','Mode':'TwoWay'}}" },
                        { SubTotalText, "{'Text':{'Path':'SubTotal','Converter':'Float','Mode':'TwoWay'}}" },
                    });

        }
		
        public override void ViewDidUnload ()
        {
            base.ViewDidUnload ();
			
            // Clear any references to subviews of the main view in order to
            // allow the Garbage Collector to collect them sooner.
            //
            // e.g. myOutlet.Dispose (); myOutlet = null;
			
            ReleaseDesignerOutlets ();
        }
		
        public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
        {
            // Return true for supported orientations
            return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
        }
    }
}

