using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.ViewModels;
using Playground.Core.ViewModels;

namespace Playground.WpfCore.Views
{
    /// <summary>
    /// Interaction logic for Tab3View.xaml
    /// </summary>
    [MvxRegionPresentation(RegionName = "Tab3Region", WindowIdentifier = "TabsRootView")]
    [MvxViewFor(typeof(Tab3ViewModel))]
    public partial class Tab3View
    {
        public Tab3View()
        {
            InitializeComponent();
        }
    }
}
