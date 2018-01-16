using MvvmCross.Wpf.Views;
using MvvmCross.Wpf.Views.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Wpf.Views
{
    /// <summary>
    /// Interaction logic for SecondChildView.xaml
    /// </summary>    
    [MvxContentPresentation]
    public partial class SecondChildView : MvxWpfView<SecondChildViewModel>
    {
        public SecondChildView()
        {
            InitializeComponent();
        }
    }
}
