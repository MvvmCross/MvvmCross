using MvvmCross.Platforms.Wpf.Presenters.Attributes;

namespace Playground.WpfCore.Views
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
