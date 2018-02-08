using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform.Tvos.Views;
using MvvmCross.Platform.Tvos.Views.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.TvOS
{
    [MvxFromStoryboard("Main")]
    [MvxChildPresentation]
    public partial class ChildView : MvxViewController<ChildViewModel>
	{
		public ChildView (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<ChildView, ChildViewModel>();
            set.Bind(btnClose).To(vm => vm.CloseCommand);
            set.Bind(btnShowChild).To(vm => vm.ShowSecondChildCommand);
            set.Apply();
        }
	}
}
