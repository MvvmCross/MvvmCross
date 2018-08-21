// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.ViewModels;
using MvvmCross.Views;
using Windows.UI.Xaml.Controls;

namespace MvvmCross.Platforms.Uap.Views
{
    public class MvxWindowsContentDialog
        : ContentDialog
        , IMvxView
    {
        public MvxWindowsContentDialog()
        {
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
