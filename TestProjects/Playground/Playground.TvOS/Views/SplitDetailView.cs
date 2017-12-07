using System;

using MvvmCross.Binding.BindingContext;
using MvvmCross.tvOS.Views;
using MvvmCross.tvOS.Views.Presenters.Attributes;

using Playground.Core.ViewModels;

namespace Playground.TvOS
{
    [MvxFromStoryboard("Main")]
    [MvxDetailSplitViewPresentation]
    public partial class SplitDetailView : MvxViewController<SplitDetailViewModel>
	{
		public SplitDetailView (IntPtr handle) : base (handle)
		{
		}
	}
}
