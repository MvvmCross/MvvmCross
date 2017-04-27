using Foundation;
using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using RoutingExample.Core.ViewModels;
using UIKit;
using MvvmCross.iOS.Views.Presenters.Attributes;

namespace RoutingExample.iOS
{
    [MvxFromStoryboard("Main")]
    [MvxRootPresentation(WrapInNavigationController = true)]
    public partial class MainViewController : MvxViewController<MainViewModel>
    {
        public MainViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<MainViewController, MainViewModel>();

            set.Bind(BtnTestA).To(vm => vm.ShowACommand);
            set.Bind(BtnTestB).To(vm => vm.ShowBCommand);
            set.Bind(BtnRandom).To(vm => vm.ShowRandomCommand);

            set.Apply();

        }
    }
}