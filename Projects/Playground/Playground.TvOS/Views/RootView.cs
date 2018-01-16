using System;

using MvvmCross.Binding.BindingContext;
using MvvmCross.tvOS.Views;
using MvvmCross.tvOS.Views.Presenters.Attributes;
using Playground.Core.ViewModels;

using Foundation;
using UIKit;

namespace Playground.TvOS
{
    [MvxFromStoryboard("Main")]
    [MvxRootPresentation(WrapInNavigationController = true)]
    public partial class RootView : MvxViewController<RootViewModel>
	{
		public RootView (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<RootView, RootViewModel>();
            set.Bind(btnChild).To(vm => vm.ShowChildCommand);
            set.Bind(btnTabs).To(vm => vm.ShowTabsCommand);
            set.Bind(btnModalNav).To(vm => vm.ShowModalNavCommand);
            set.Bind(btnModal).To(vm => vm.ShowModalCommand);
            set.Bind(btnModalAttribute).To(vm => vm.ShowOverrideAttributeCommand);
            set.Bind(btnSplitNav).To(vm => vm.ShowSplitCommand);
            set.Apply();
        }
	}
}
