using Foundation;
using System;
using MvvmCross.iOS.Views;
using RoutingExample.Core.ViewModels;
using UIKit;

namespace RoutingExample.iOS
{
    [MvxFromStoryboard("Main")]
    public partial class TestAViewController : MvxViewController<TestAViewModel>
    {
        public TestAViewController (IntPtr handle) : base (handle)
        {
        }
    }
}