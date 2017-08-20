using MvvmCross.Core.ViewModels;
using MvvmCross.Wpf.Views;
using Playground.Core.ViewModels;

namespace Playground.Wpf.Views
{
    /// <summary>
    /// Interaction logic for ChildView.xaml
    /// </summary>
    [MvxViewFor(typeof(ChildViewModel))]
    public partial class ChildView : MvxWpfView
    {
        public ChildView()
        {
            InitializeComponent();            
        }
    }
}
