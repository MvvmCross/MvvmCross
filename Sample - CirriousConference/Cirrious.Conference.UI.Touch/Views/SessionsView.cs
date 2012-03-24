using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.Conference.Core.ViewModels.HomeViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Binding.Touch.ExtensionMethods;
using System.Collections.Generic;

namespace Cirrious.Conference.UI.Touch
{
    public partial class SessionsView
        : MvxBindingTouchViewController<SessionsViewModel>
    {
        public SessionsView(MvxShowViewModelRequest request)
            : base(request, "SessionsView", null)
        {
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

            NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Tweet", UIBarButtonItemStyle.Bordered, (sender, e) => ViewModel.ShareGeneralCommand.Execute()), false);

            // Perform any additional setup after loading the view, typically from a nib.
            Button1.SetImage(UIImage.FromFile("ConfResources/Images/appbar.calendar.png"), UIControlState.Normal);
            Button2.SetImage(UIImage.FromFile("ConfResources/Images/appbar.calendar.png"), UIControlState.Normal);
            Button3.SetImage(UIImage.FromFile("ConfResources/Images/appbar.calendar.png"), UIControlState.Normal);
            Button4.SetImage(UIImage.FromFile("ConfResources/Images/appbar.people.png"), UIControlState.Normal);
            Button5.SetImage(UIImage.FromFile("ConfResources/Images/appbar.database.png"), UIControlState.Normal);

            this.AddBindings(new Dictionary<object, string>()
			    {
					{ Label1, "{'Text':{'Path':'TextSource','Converter':'Language','ConverterParameter':'ByDay'}}" },				
					{ Label2, "{'Text':{'Path':'TextSource','Converter':'Language','ConverterParameter':'BySpeaker'}}" },				
					{ Label3, "{'Text':{'Path':'TextSource','Converter':'Language','ConverterParameter':'ByTopic'}}" },				
					{ Button1, "{'Title':{'Path':'TextSource','Converter':'Language','ConverterParameter':'Thursday'}}" },				
					{ Button2, "{'Title':{'Path':'TextSource','Converter':'Language','ConverterParameter':'Friday'}}" },				
					{ Button3, "{'Title':{'Path':'TextSource','Converter':'Language','ConverterParameter':'Saturday'}}" },				
					{ Button4, "{'Title':{'Path':'TextSource','Converter':'Language','ConverterParameter':'Speakers'}}" },				
					{ Button5, "{'Title':{'Path':'TextSource','Converter':'Language','ConverterParameter':'Topics'}}" },				
				});

            this.AddBindings(new Dictionary<object, string>()
			    {
					{ Button1, "{'TouchDown':{'Path':'ShowThursdayCommand'}}" },				
					{ Button2, "{'TouchDown':{'Path':'ShowFridayCommand'}}" },				
					{ Button3, "{'TouchDown':{'Path':'ShowSaturdayCommand'}}" },				
					{ Button4, "{'TouchDown':{'Path':'ShowSpeakersCommand'}}" },				
					{ Button5, "{'TouchDown':{'Path':'ShowTopicsCommand'}}" },				
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

