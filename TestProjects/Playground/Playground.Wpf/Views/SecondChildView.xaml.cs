using MvvmCross.Core.ViewModels;
using MvvmCross.Wpf.Views;
using Playground.Core.ViewModels;

namespace Playground.Wpf.Views
{
    /// <summary>
    /// Interaction logic for SecondChildView.xaml
    /// </summary>
    [MvxViewFor(typeof(SecondChildViewModel))]
    public partial class SecondChildView : MvxWpfView
    {
        public SecondChildView()
        {
            InitializeComponent();
        }
    }
}
