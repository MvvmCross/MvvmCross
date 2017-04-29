// MvxWpfView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Windows;
using System.Windows.Controls;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Wpf.Views
{
    public class MvxWpfView : UserControl, IMvxWpfView, IDisposable
    {
        private IMvxViewModel _viewModel;

        public MvxWpfView()
        {
            Unloaded += MvxWpfView_Unloaded;
            Loaded += MvxWpfView_Loaded;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IMvxViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                _viewModel = value;
                DataContext = value;
            }
        }

        private void MvxWpfView_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.Disappearing();
            ViewModel?.Disappeared();
        }

        private void MvxWpfView_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.Appearing();
            ViewModel?.Appeared();
        }

        ~MvxWpfView()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Unloaded -= MvxWpfView_Unloaded;
                Loaded -= MvxWpfView_Loaded;
            }
        }
    }

    public class MvxWpfView<TViewModel>
        : MvxWpfView
            , IMvxWpfView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get => (TViewModel) base.ViewModel;
            set => base.ViewModel = value;
        }
    }
}