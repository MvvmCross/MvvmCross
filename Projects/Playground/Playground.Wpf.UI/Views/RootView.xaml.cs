using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.ViewModels;
using Playground.Core.ViewModels;

namespace Playground.Wpf.UI.Views
{
    [MvxContentPresentation]
    [MvxViewFor(typeof(Playground.Wpf.UI.ViewModels.RootViewModel))]
    public partial class RootView
    {
        public RootView()
        {
            InitializeComponent();
        }
    }
}
