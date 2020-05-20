using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.ViewModels;
using Playground.Core.ViewModels;

namespace Playground.WpfCore.Views
{
    /// <summary>
    /// Interaction logic for Tab2View.xaml
    /// </summary>
    [MvxRegionPresentation(RegionName = "Tab2Region", WindowIdentifier = "TabsRootView")]
    [MvxViewFor(typeof(Tab2ViewModel))]
    public partial class Tab2View
    {
        public Tab2View()
        {
            InitializeComponent();
        }
    }
}
