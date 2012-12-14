using System.Windows.Controls;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Wpf.Interfaces;

namespace Cirrious.MvvmCross.Wpf.Views
{
    public class MvxWpfView : UserControl, IMvxWpfView
    {
        // TODO - warning IMvxView.IsVisible is implemented here by UserControl! 

        private IMvxViewModel _viewModel;
        public IMvxViewModel ViewModel
        {
            get { return _viewModel; }
            set { _viewModel = value; DataContext = value; }
        }
    }
}