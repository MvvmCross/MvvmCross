using MvvmCross.Core.ViewModels;
using MvvmCross.Wpf.Views;
using Playground.Core.ViewModels;

namespace Playground.Wpf.Views
{
    /// <summary>
    /// Interaction logic for RootView.xaml
    /// </summary>
    [MvxViewFor(typeof(RootViewModel))]
    public partial class RootView : MvxWpfView
    {
        public RootView()
        {
            InitializeComponent();
        }
    }
}
