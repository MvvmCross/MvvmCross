using MvvmCross.Wpf.Views;
using MvvmCross.Wpf.Views.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Wpf.Views
{
    /// <summary>
    /// Interaction logic for ModalView.xaml
    /// </summary>
    [MvxWindowPresentation(ViewModelType = typeof(ModalViewModel), Identifier = nameof(ModalView), Modal = true)]
    public partial class ModalView : MvxWpfView
    {
        public ModalView()
        {
            InitializeComponent();
        }
    }
}
