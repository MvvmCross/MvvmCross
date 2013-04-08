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
using Cirrious.CrossCore.Core;

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

            var bindings = this.CreateBindingSet<HomeView, HomeViewModel>();
            bindings.Bind(this.Go).To(vm => vm.Commands["Search"]);
            bindings.Bind(this.Random).To(vm => vm.Commands["PickRandom"]);
            bindings.Bind(this.Edit).To(vm => vm.SearchText);
            bindings.Apply();

            /*
			var bindings = new List<IMvxApplicable>()
			{
				this.CreateBinding(this.Go).To<HomeViewModel>(vm => vm.Commands["Search"]),
	            this.CreateBinding(this.Random).To<HomeViewModel>(vm => vm.Commands["PickRandom"]),
	            this.CreateBinding(this.Edit).To<HomeViewModel>(vm => vm.SearchText)
			};
			bindings.Apply();
             */
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

