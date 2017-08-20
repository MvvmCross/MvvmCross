using MvvmCross.Wpf.Views;
using MvvmCross.Wpf.Views.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Wpf.Views
{
    /// <summary>
    /// Interaction logic for NestedModalView.xaml
    /// </summary>
    [MvxContentPresentation(ViewModelType = typeof(NestedModalViewModel), WindowIdentifier = nameof(ModalView))]
    public partial class NestedModalView : MvxWpfView
    {
        public NestedModalView()
        {
            InitializeComponent();
        }
    }
}
