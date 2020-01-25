using System;
using MvvmCross.Platforms.Tvos.Presenters.Attributes;
using MvvmCross.Platforms.Tvos.Views;
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

            var set = CreateBindingSet();
            set.Bind(btnClose).To(vm => vm.CloseCommand);
            set.Bind(btnShowChild).To(vm => vm.ShowSecondChildCommand);
            set.Apply();
        }
	}
}
