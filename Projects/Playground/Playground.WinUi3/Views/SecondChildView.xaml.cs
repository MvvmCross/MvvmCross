using MvvmCross.Platforms.WinUi.Presenters.Attributes;

namespace Playground.WinUi.Views
{
    [MvxRegionPresentation("NestedFrame")]
    public sealed partial class SecondChildView
    {
        public SecondChildView()
        {
            InitializeComponent();
        }
    }
}
