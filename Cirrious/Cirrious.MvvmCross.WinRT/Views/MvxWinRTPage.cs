// MvxWinRTPage.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WinRT.Interfaces;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Cirrious.MvvmCross.WinRT.Views
{
    public abstract class MvxWinRTPage
        : Page
          , IMvxWinRTView
    {
        private IMvxViewModel _viewModel;

        #region IMvxWindowsPhoneView<T> Members

        public bool IsVisible { get; set; }

        public IMvxViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                if (_viewModel == value)
                    return;

                _viewModel = value;
                DataContext = ViewModel;
            }
        }

        public void ClearBackStack()
        {
            throw new NotImplementedException();
            /*
            // note - we do *not* use CanGoBack here - as that seems to always returns true!
            while (NavigationService.BackStack.Any())
                NavigationService.RemoveBackEntry();
         */
        }

        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            IsVisible = true;

            this.OnViewCreate(e.Parameter as MvxShowViewModelRequest);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            IsVisible = false;
            base.OnNavigatedFrom(e);

            if (e.NavigationMode == NavigationMode.Back)
                this.OnViewDestroy();
        }
    }
}