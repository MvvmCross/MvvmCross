using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.ViewModels;
using Playground.Core.ViewModels;

namespace Playground.Wpf.Views
{
    //[MvxContentPresentation]
    [MvxViewFor(typeof(RootViewModel))]
    public partial class RootView
    {
        public RootView()
        {
            InitializeComponent();
        }
    }
}
