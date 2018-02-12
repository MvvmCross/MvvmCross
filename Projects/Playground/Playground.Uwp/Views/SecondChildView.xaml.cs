using MvvmCross.Platform.Uap.Presenters.Attributes;

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
