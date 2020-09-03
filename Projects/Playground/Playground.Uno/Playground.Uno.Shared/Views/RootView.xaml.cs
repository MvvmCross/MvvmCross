using MvvmCross.Platforms.Uap.Presenters.Attributes;
using MvvmCross.ViewModels;
using Playground.Core.ViewModels;

namespace Playground.Uwp.Views
{
    [MvxViewFor(typeof(RootViewModel))]
    [MvxPagePresentation]
    public sealed partial class RootView : BaseRootViewPage
    {
        public RootView()
        {
            this.InitializeComponent();
        }
    }
}
