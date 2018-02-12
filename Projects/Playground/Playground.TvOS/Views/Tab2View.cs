using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform.Tvos.Presenters.Attributes;
using MvvmCross.Platform.Tvos.Views;
using Playground.Core.ViewModels;

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
