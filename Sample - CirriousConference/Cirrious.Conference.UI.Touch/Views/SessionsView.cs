using System;
using System.Drawing;
using Cirrious.MvvmCross.Binding.Touch;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.Conference.Core.ViewModels.HomeViewModels;
using Cirrious.MvvmCross.Views;
using System.Collections.Generic;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace Cirrious.Conference.UI.Touch
{
    public partial class SessionsView
        : MvxViewController
    {
        public SessionsView()
            : base("SessionsView", null)
        {
        }

		public new SessionsViewModel ViewModel {
			get { return (SessionsViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Tweet", UIBarButtonItemStyle.Bordered, (sender, e) => ViewModel.DoShareGeneral()), false);

            // Perform any additional setup after loading the view, typically from a nib.
            Button1.SetImage(UIImage.FromFile("ConfResources/Images/appbar.calendar.png"), UIControlState.Normal);
            Button2.SetImage(UIImage.FromFile("ConfResources/Images/appbar.calendar.png"), UIControlState.Normal);
            Button3.SetImage(UIImage.FromFile("ConfResources/Images/appbar.calendar.png"), UIControlState.Normal);
            Button4.SetImage(UIImage.FromFile("ConfResources/Images/appbar.people.png"), UIControlState.Normal);
            Button5.SetImage(UIImage.FromFile("ConfResources/Images/appbar.database.png"), UIControlState.Normal);

            this.AddBindings(new Dictionary<object, string>()
			    {
					{ Label1, "Text TextSource,Converter=Language, ConverterParameter='ByDay'" },				
					{ Label2, "Text TextSource,Converter=Language, ConverterParameter='BySpeaker'" },				
					{ Label3, "Text TextSource,Converter=Language, ConverterParameter='ByTopic'" },				
					{ Button1, "Title TextSource,Converter=Language, ConverterParameter='Thursday'" },				
					{ Button2, "Title TextSource,Converter=Language, ConverterParameter='Friday'" },				
					{ Button3, "Title TextSource,Converter=Language, ConverterParameter='Saturday'" },				
					{ Button4, "Title TextSource,Converter=Language, ConverterParameter='Speakers'" },				
					{ Button5, "Title TextSource,Converter=Language, ConverterParameter='Topics'" },				
				});

            this.AddBindings(new Dictionary<object, string>()
			    {
					{ Button1, "TouchUpInside ShowThursdayCommand" },				
					{ Button2, "TouchUpInside ShowFridayCommand" },				
					{ Button3, "TouchUpInside ShowSaturdayCommand" },				
					{ Button4, "TouchUpInside ShowSpeakersCommand" },				
					{ Button5, "TouchUpInside ShowTopicsCommand" },				
				});
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

