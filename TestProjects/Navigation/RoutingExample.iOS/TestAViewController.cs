using Foundation;
using System;
using MvvmCross.iOS.Views;
using RoutingExample.Core.ViewModels;
using UIKit;
using MvvmCross.iOS.Views.Presenters.Attributes;

namespace RoutingExample.iOS
{
    [MvxFromStoryboard("Main")]
    [MvxChildPresentation]
    public partial class TestAViewController : MvxViewController<TestAViewModel>
    {
        public TestAViewController(IntPtr handle) : base(handle)
        {
        }
    }
}