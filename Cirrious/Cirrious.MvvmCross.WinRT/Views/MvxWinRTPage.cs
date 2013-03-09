// MvxWinRTPage.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.ViewModels;
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

        private String _pageKey;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            IsVisible = true;

#warning Would be nice to refactor frameState and MvxSuspensionManager a little...
            var frameState = MvxSuspensionManager.SessionStateForFrame(this.Frame);
            _pageKey = "Page-" + this.Frame.BackStackDepth;
            IMvxBundle bundle = null;

            if (e.NavigationMode == NavigationMode.New)
            {
                // Clear existing state for forward navigation when adding a new page to the
                // navigation stack
                var nextPageKey = this._pageKey;
                int nextPageIndex = this.Frame.BackStackDepth;
                while (frameState.Remove(nextPageKey))
                {
                    nextPageIndex++;
                    nextPageKey = "Page-" + nextPageIndex;
                }
            }
            else
            {
                var dictionary = (Dictionary<string, string>) frameState[this._pageKey];
                bundle = new MvxBundle(dictionary);
            }

            this.OnViewCreate(e.Parameter as MvxShowViewModelRequest, bundle);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            IsVisible = false;
            base.OnNavigatedFrom(e);

            var frameState = MvxSuspensionManager.SessionStateForFrame(this.Frame);
            var bundle = this.CreateSaveStateBundle();
            frameState[_pageKey] = bundle.Data;

            if (e.NavigationMode == NavigationMode.Back)
                this.OnViewDestroy();
        }
    }
}