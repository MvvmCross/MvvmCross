using MvvmCross.Platforms.Wpf.Presenters.Attributes;

namespace Playground.Wpf.Views
{
    [MvxWindowPresentation(Identifier = nameof(ModalView), Modal = true)]
    public partial class ModalView
    {
        public ModalView()
        {
            InitializeComponent();
        }
    }
}
