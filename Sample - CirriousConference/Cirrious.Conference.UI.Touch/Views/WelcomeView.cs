using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.Conference.Core.ViewModels.HomeViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Binding.Touch.ExtensionMethods;

namespace Cirrious.Conference.UI.Touch
{
    public partial class WelcomeView 
		: MvxBindingViewController
    {
        public WelcomeView()
			: base("WelcomeView", null)
        {
        }

		public new WelcomeViewModel ViewModel {
			get { return (WelcomeViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.

            Button1.SetImage(UIImage.FromFile("ConfResources/Images/appbar.people.png"), UIControlState.Normal);
            Button2.SetImage(UIImage.FromFile("ConfResources/Images/appbar.city.png"), UIControlState.Normal);
            Button3.SetImage(UIImage.FromFile("ConfResources/Images/appbar.bus.png"), UIControlState.Normal);
            Button4.SetImage(UIImage.FromFile("ConfResources/Images/appbar.questionmark.rest.png"), UIControlState.Normal);

            this.AddBindings(new Dictionary<object, string>()
                {
                    { MainLabel, "{'Text':{'Path':'TextSource','Converter':'Language','ConverterParameter':'AboutSQLBits'}}" },				
                    { Button1, "{'Title':{'Path':'TextSource','Converter':'Language','ConverterParameter':'Sponsors'}}" },				
                    { Button2, "{'Title':{'Path':'TextSource','Converter':'Language','ConverterParameter':'Exhibitors'}}" },				
                    { Button3, "{'Title':{'Path':'TextSource','Converter':'Language','ConverterParameter':'Map'}}" },				
                    { Button4, "{'Title':{'Path':'TextSource','Converter':'Language','ConverterParameter':'About'}}" },				
                });

            this.AddBindings(new Dictionary<object, string>()
                {
                    { Button1, "{'TouchUpInside':{'Path':'ShowSponsorsCommand'}}" },				
                    { Button2, "{'TouchUpInside':{'Path':'ShowExhibitorsCommand'}}" },				
                    { Button3, "{'TouchUpInside':{'Path':'ShowMapCommand'}}" },				
                    { Button4, "{'TouchUpInside':{'Path':'ShowAboutCommand'}}" },				
                });

            NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Tweet", UIBarButtonItemStyle.Bordered, (sender, e) => ViewModel.DoShareGeneral()), false);
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
            ReleaseDesignerOutlets();
        }

        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            // Return true for supported orientations
            return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
        }
    }
}

