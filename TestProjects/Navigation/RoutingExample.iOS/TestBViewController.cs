using Foundation;
using System;
using MvvmCross.iOS.Views;
using RoutingExample.Core.ViewModels;
using UIKit;

namespace RoutingExample.iOS
{
    [MvxFromStoryboard("Main")]
    public partial class TestBViewController : MvxViewController<TestBViewModel>
    {
        public TestBViewController (IntPtr handle) : base (handle)
        {
        }
    }
}