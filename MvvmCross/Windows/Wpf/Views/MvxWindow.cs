using MvvmCross.Core.ViewModels;
using System;
using System.Windows;

namespace MvvmCross.Wpf.Views
{
    public class MvxWindow : Window, IMvxWindow, IMvxWpfView, IDisposable
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

        public string Identifier { get; set; }

        public MvxWindow()
        {
            Unloaded += MvxWindow_Unloaded;
            Loaded += MvxWindow_Loaded;
        }

        private void MvxWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.ViewDisappearing();
            ViewModel?.ViewDisappeared();
        }

        private void MvxWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.ViewAppearing();
            ViewModel?.ViewAppeared();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MvxWindow()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Unloaded -= MvxWindow_Unloaded;
                Loaded -= MvxWindow_Loaded;
            }
        }
    }

    public class MvxWindow<TViewModel>
        : MvxWindow
          , IMvxWpfView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}