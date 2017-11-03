using MvvmCross.Core.ViewModels;
using MvvmCross.Uwp.Attributes;
using MvvmCross.Uwp.Views;
using Playground.Core.ViewModels;

namespace Playground.Uwp.Views
{
    [MvxViewFor(typeof(RootViewModel))]
    [ContentPagePresentation]
    public sealed partial class RootView : MvxWindowsPage
    {
        public RootView()
        {
            this.InitializeComponent();
        }
    }
}
