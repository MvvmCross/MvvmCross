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
    [MvxTabPresentation]
    public partial class Tab2View : MvxViewController<Tab2ViewModel>
	{
		public Tab2View (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<Tab2View, Tab2ViewModel>();

            set.Bind(btnStackNav).To(vm => vm.ShowRootViewModelCommand);
            set.Bind(btnClose).To(vm => vm.CloseViewModelCommand);

            set.Apply();
        }
	}
}