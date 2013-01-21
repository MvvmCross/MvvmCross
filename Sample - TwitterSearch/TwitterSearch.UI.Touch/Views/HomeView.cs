using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using TwitterSearch.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Touch.ExtensionMethods;
using System.Collections.Generic;
using Cirrious.MvvmCross.Views;

namespace TwitterSearch.UI.Touch.Views
{
    public sealed partial class HomeView 
        : MvxBindingTouchViewController<HomeViewModel>
    {
        public HomeView (MvxShowViewModelRequest request)
            : base (request, "HomeView", null)
        {
            Title = "Home";
        }
        
        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            
            this.AddBindings(new Dictionary<object, string>()
            {
                { Go, "TouchDown SearchCommand"}, 
                { Random, "TouchDown PickRandomCommand"}, 
                { Edit, "Text SearchText"}, 
            });
        }
        
        public override void ViewDidUnload ()
        {
            base.ViewDidUnload ();
            
            ReleaseDesignerOutlets ();
        }
        
        public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
        {
            // Return true for supported orientations
            return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
        }
    }
}

