using System;
using System.Collections.Generic;
using System.Drawing;
using Cirrious.MvvmCross.Binding.Touch;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.Conference.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace Cirrious.Conference.UI.Touch
{
    public partial class SessionView : MvxViewController
    {
        public SessionView()
            : base("SessionView", null)
        {
        }

		public new SessionViewModel ViewModel {
			get { return (SessionViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            TextView1.Editable = false;
            TextView1.Text = "Conference";
            ImageView1.Image = UIImage.FromFile("ConfResources/Images/appbar.people.png");
            ImageView2.Image = UIImage.FromFile("ConfResources/Images/appbar.city.png");

            this.AddBindings(new Dictionary<object, string>()
                                 {
                                     {Label1, "Text Session.Session.Title"},
                                     {TextView1, "Text Session.Session.Description"},
                                     {SubLabel1, "Text Session.Session.SpeakerKey"},
                                     {SubLabel2, "Text Session.Session,Converter=SessionSmallDetails,ConverterParameter='SmallDetailsFormat'"},
                                     {favoriteButton,"IsFavorite Session.IsFavorite"}
                                 });

            NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Tweet", UIBarButtonItemStyle.Bordered, (sender, e) => ViewModel.DoShare()), false);

        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();

            // Clear any references to subviews of the main view in order to
            // allow the Garbage Collector to collect them sooner.
            //
            // e.g. myOutlet.Dispose (); myOutlet = null;

            ReleaseDesignerOutlets();
        }

        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            // Return true for supported orientations
            return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
        }
    }
}

