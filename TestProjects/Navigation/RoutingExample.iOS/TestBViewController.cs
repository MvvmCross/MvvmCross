using System;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using RoutingExample.Core.ViewModels;

namespace RoutingExample.iOS
{
    [MvxFromStoryboard("Main")]
    [MvxChildPresentation]
    public partial class TestBViewController : MvxViewController<TestBViewModel>
    {
        public TestBViewController(IntPtr handle) : base(handle)
        {
        }
    }
}