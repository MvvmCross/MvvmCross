using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform.Tvos.Views;
using MvvmCross.Platform.Tvos.Views.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.TvOS
{
    [MvxFromStoryboard("Main")]
    [MvxTabPresentation(WrapInNavigationController = false)]
    public partial class Tab3View : MvxViewController<Tab3ViewModel>, IMvxTabBarItemViewController
	{
        public string TabName => "Third";
        public string TabIconName => "settings";

        public string TabSelectedIconName => "settings";

		public Tab3View (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<Tab3View, Tab3ViewModel>();

            set.Bind(btnStackNav).To(vm => vm.ShowRootViewModelCommand);
            set.Bind(btnClose).To(vm => vm.CloseViewModelCommand);

            set.Apply();
        }
	}
}
