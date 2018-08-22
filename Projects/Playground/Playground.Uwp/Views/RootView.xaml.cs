using MvvmCross.Platforms.Uap.Presenters.Attributes;
using MvvmCross.Platforms.Uap.Views;
using MvvmCross.ViewModels;
using Playground.Core.ViewModels;

namespace Playground.Uwp.Views
{
    [MvxViewFor(typeof(RootViewModel))]
    [MvxPagePresentation]
    public sealed partial class RootView : MvxWindowsPage
    {
        public RootView()
        {
            this.InitializeComponent();
        }
    }
}
