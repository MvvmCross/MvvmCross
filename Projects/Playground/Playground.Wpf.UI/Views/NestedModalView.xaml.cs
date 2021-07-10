using MvvmCross.Platforms.Wpf.Presenters.Attributes;

namespace Playground.Wpf.UI.Views
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