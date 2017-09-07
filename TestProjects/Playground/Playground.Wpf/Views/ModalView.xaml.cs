using MvvmCross.Wpf.Views;
using MvvmCross.Wpf.Views.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Wpf.Views
{
    /// <summary>
    /// Interaction logic for ModalView.xaml
    /// </summary>
    [MvxWindowPresentation(Identifier = nameof(ModalView), Modal = true)]
    public partial class ModalView : MvxWpfView<ModalViewModel>
    {
        public ModalView()
        {
            InitializeComponent();
        }
    }
}
