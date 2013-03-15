using System;
using System.Drawing;
using Cirrious.MvvmCross.Binding.Touch;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Tutorial.Core.ViewModels.Lessons;
using Cirrious.MvvmCross.Binding.Touch.Views;
using System.Collections.Generic;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace Tutorial.UI.Touch.Views
{
    public partial class TipView : MvxViewController
    {
        public TipView () : base ("TipView", null)
        {
        }

		public new TipViewModel ViewModel {
			get { return (TipViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            this.AddBindings(
                new Dictionary<object, string>()
                    {
                        { TipValueLabel, "Text TipValue" },
                        { TotalLabel, "Text Total" },
                        { TipPercentText, "Text TipPercent,Converter=Int,Mode=TwoWay" },
                        { TipPercentSlider, "Value TipPercent,Converter=IntToFloat,Mode=TwoWay" },
                        { SubTotalText, "Text SubTotal,Converter=Float,Mode=TwoWay" },
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

