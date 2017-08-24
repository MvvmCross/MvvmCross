using MvvmCross.Wpf.Views;
using MvvmCross.Wpf.Views.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Wpf.Views
{
    /// <summary>
    /// Interaction logic for ChildView.xaml
    /// </summary>
    [MvxContentPresentation]
    public partial class ChildView : MvxWpfView<ChildViewModel>
    {
        public ChildView()
        {
            InitializeComponent();            
        }
    }
}
