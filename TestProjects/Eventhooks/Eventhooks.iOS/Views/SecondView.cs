using System;
using Eventhooks.Core.ViewModels;
using MvvmCross.iOS.Views;
using UIKit;
using MvvmCross.iOS.Views.Presenters.Attributes;

namespace Eventhooks.iOS
{
    [MvxChildPresentation]
    public partial class SecondView : MvxViewController<SecondViewModel>
    {
        public SecondView() : base("SecondView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

