using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.ViewModels;
using Playground.Core.ViewModels;

namespace Playground.WpfCore.Views
{
    /// <summary>
    /// Interaction logic for Tab1View.xaml
    /// </summary>
    [MvxRegionPresentation(RegionName = "Tab1Region", WindowIdentifier = "TabsRootView")]
    [MvxViewFor(typeof(Tab1ViewModel))]
    public partial class Tab1View
    {
        public Tab1View()
        {
            InitializeComponent();
        }
    }
}
