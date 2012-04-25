using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements;
using Cirrious.MvvmCross.Views;
using MonoTouch.UIKit;
using Tutorial.Core.ViewModels.Lessons;

namespace Tutorial.UI.Touch.Views.Lessons
{
    public class LocationView
         : MvxTouchDialogViewController<LocationViewModel>
    {
        public LocationView(MvxShowViewModelRequest request) 
            : base(request, UITableViewStyle.Grouped, null, true)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Bordered, null), false);
            this.NavigationItem.LeftBarButtonItem.Clicked += delegate
            {
                ViewModel.CloseCommand.Execute();
            };

            this.Root = new RootElement("Location Property")
                            {
                                new Section("Status")
                                    {
                                        new StringElement("Started").Bind(this, "{'Value':{'Path':'IsStarted'}}"),
                                        new StringElement("Error?").Bind(this, "{'Value':{'Path':'LastError'}"),
                                    },
                                new Section("Location")
                                    {
                                        new StringElement("Lat").Bind(this, "{'Value':{'Path':'Lat'}}"),
                                        new StringElement("Lng").Bind(this, "{'Value':{'Path':'Lng'}}"),
                                    },
                                new Section("Control")
                                    {
                                        new StringElement("Start/Stop").Bind(this, "{'SelectedCommand':{'Path':'StartStopCommand'}}"),
                                    },
                            };
        }
    }
}
