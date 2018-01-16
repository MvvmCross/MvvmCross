using MvvmCross.Uwp.Attributes;

namespace Playground.Uwp.Views
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
