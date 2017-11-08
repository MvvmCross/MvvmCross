using MvvmCross.Uwp.Attributes;

namespace Playground.Uwp.Views
{
    [MvxRegionPresentation("Nested")]
    public sealed partial class SecondChildView 
    {
        public SecondChildView()
        {
            InitializeComponent();
        }
    }
}
