using MvvmCross.Platforms.Wpf.Presenters.Attributes;

namespace Playground.Wpf.Views
{
    [MvxContentPresentation(WindowIdentifier = nameof(ModalView))]
    public partial class NestedModalView
    {
        public NestedModalView()
        {
            InitializeComponent();
        }
    }
}
