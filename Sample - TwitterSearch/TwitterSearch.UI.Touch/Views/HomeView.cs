using System;
using System.Drawing;
using Cirrious.MvvmCross.Binding.Touch;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using TwitterSearch.Core.ViewModels;
using System.Collections.Generic;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace TwitterSearch.UI.Touch.Views
{
    public sealed partial class HomeView 
        : MvxViewController
    {
        public HomeView ()
            : base ("HomeView", null)
        {
            Title = "Home";
        }
        
		public new HomeViewModel ViewModel {
			get { return (HomeViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            
			this.Bind (this.Go, (HomeViewModel vm) => vm.Commands["Search"]);
			this.Bind (this.Random, (HomeViewModel vm) => vm.Commands["PickRandom"]);
			this.Bind (this.Edit, (HomeViewModel vm) => vm.SearchText);
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

