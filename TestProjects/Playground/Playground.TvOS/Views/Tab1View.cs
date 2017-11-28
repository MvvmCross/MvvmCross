using System;

using MvvmCross.Binding.BindingContext;
using MvvmCross.tvOS.Views;
using MvvmCross.tvOS.Views.Presenters.Attributes;

using Playground.Core.ViewModels;

using UIKit;
using Foundation;

namespace Playground.TvOS
{
    [MvxFromStoryboard("Main")]
    [MvxTabPresentation(WrapInNavigationController = true, TabIconName = "home", TabName = "Tab 1")]
    public partial class Tab1View : MvxViewController<Tab1ViewModel>
	{
		public Tab1View (IntPtr handle) : base (handle)
		{
		}


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<Tab1View, Tab1ViewModel>();

            set.Bind(btnModal).To(vm => vm.OpenModalCommand);
            set.Bind(btnModalNav).To(vm => vm.OpenNavModalCommand);
            set.Bind(btnChild).To(vm => vm.OpenChildCommand);

            set.Apply();

        }
	}
}
