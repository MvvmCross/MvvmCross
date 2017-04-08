using Foundation;
using System;
using UIKit;
using Playground.Core.ViewModels;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;

namespace Playground.iOS.Views
{
    [MvxFromStoryboard("Main")]
    [MvxTabPresentation]
    public partial class Tab2View : MvxViewController<Tab2ViewModel>
    {
        public Tab2View(IntPtr handle) : base(handle)
        {
        }
    }
}