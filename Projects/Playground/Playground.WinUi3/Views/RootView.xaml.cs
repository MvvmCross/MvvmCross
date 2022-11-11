using MvvmCross.Platforms.WinUi.Presenters.Attributes;
using MvvmCross.Platforms.WinUi.Views;
using MvvmCross.ViewModels;
using Playground.Core.ViewModels;

namespace Playground.WinUi.Views
{
    [MvxViewFor(typeof(RootViewModel))]
    [MvxPagePresentation]
    public sealed partial class RootView : RootViewPage
    {
        public RootView()
        {
            this.InitializeComponent();
        }
    }

    public abstract class RootViewPage : MvxWindowsPage<RootViewModel>
    {
    }
}
