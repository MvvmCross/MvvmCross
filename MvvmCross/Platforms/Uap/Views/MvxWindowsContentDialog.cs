// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.ViewModels;
using MvvmCross.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MvvmCross.Platforms.Uap.Views
{
    public class MvxWindowsContentDialog
        : ContentDialog
        , IMvxView
    {
        public MvxWindowsContentDialog()
        {
            Loading += MvxWindowsContentDialog_Loading;
            Loaded += MvxWindowsContentDialog_Loaded;
            Opened += MvxWindowsContentDialog_Opened;
            Unloaded += MvxWindowsContentDialog_Unloaded;
            Closed += MvxWindowsContentDialog_Closed;
            Closing += MvxWindowsContentDialog_Closing;
        }

        private void MvxWindowsContentDialog_Loading(FrameworkElement sender, object args)
        {
            ViewModel?.ViewAppearing();
        }

        private void MvxWindowsContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.ViewAppeared();
        }

        private void MvxWindowsContentDialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {
            ViewModel?.ViewCreated();
        }

        private void MvxWindowsContentDialog_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.ViewDestroy();
        }

        private void MvxWindowsContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            ViewModel?.ViewDisappearing();
        }

        private void MvxWindowsContentDialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            ViewModel?.ViewDisappeared();
        }

        private IMvxViewModel _viewModel;

        public IMvxViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                if (_viewModel == value)
                    return;

                _viewModel = value;
                DataContext = ViewModel;
            }
        }
    }

    public class MvxWindowsContentDialog<TViewModel>
        : MvxWindowsContentDialog
        , IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get => (TViewModel)base.ViewModel;
            set => base.ViewModel = value;
        }
    }
}
