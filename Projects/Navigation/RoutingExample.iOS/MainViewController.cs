using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using RoutingExample.Core.ViewModels;

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

            //set.Bind(ResultLabel).To(vm => vm.Result);
            set.Bind(BtnTestA).To(vm => vm.ShowACommand);
            set.Bind(BtnTestB).To(vm => vm.ShowBCommand);
            //set.Bind(BtnTestC).To(vm => vm.ShowCCommand);
            set.Bind(BtnRandom).To(vm => vm.ShowRandomCommand);
            set.Bind(BtnPrePop).To(vm => vm.ShowPrePopCommand);

            set.Apply();

        }
    }
}