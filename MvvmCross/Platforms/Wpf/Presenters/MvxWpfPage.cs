using System.Windows.Controls;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Wpf.Presenters
{
    public class MvxWpfPage : Page, IMvxWpfView
    {
        private IMvxViewModel _viewModel;

        public IMvxViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                _viewModel = value;
                DataContext = value;
            }
        }
    }

    public class MvxWpfPage<TViewModel>
        : MvxWpfPage, IMvxWpfView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get => (TViewModel)base.ViewModel;
            set => base.ViewModel = value;
        }
    }
}
