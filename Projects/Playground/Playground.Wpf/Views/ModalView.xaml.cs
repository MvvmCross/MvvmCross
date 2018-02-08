using MvvmCross.Platform.Wpf.Views.Presenters.Attributes;

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
