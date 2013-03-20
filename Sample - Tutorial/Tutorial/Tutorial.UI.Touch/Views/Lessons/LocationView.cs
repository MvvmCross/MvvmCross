using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.Views;
using MonoTouch.UIKit;
using Tutorial.Core.ViewModels.Lessons;
using CrossUI.Touch.Dialog.Elements;

namespace Tutorial.UI.Touch.Views.Lessons
{
    public class LocationView
         : MvxDialogViewController
    {
        public LocationView() 
            : base(UITableViewStyle.Grouped, null, true)
        {
        }

		public new LocationViewModel ViewModel {
			get { return (LocationViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Bordered, null), false);
            this.NavigationItem.LeftBarButtonItem.Clicked += delegate
            {
				this.NavigationController.PopViewControllerAnimated(true);
            };

            this.Root = new RootElement("Location Property")
                            {
                                new Section("Status")
                                    {
                                        new StringElement("Started").Bind(this, "Value IsStarted"),
					                    new StringElement("Error?").Bind(this, "Value LastError"),
                                    },
                                new Section("Location")
                                    {
                                        new StringElement("Lat").Bind(this, "Value Lat"),
                                        new StringElement("Lng").Bind(this, "Value Lng"),
                                    },
                                new Section("Control")
                                    {
										new StringElement("Start/Stop") 
										{ 
											ShouldDeselectAfterTouch = true 
										}.Bind(this, "SelectedCommand StartStopCommand"),
                                    },
                            };
        }
    }
}
