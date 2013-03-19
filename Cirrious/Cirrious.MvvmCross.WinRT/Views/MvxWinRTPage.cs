﻿// MvxWinRTPage.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WinRT.Views.Suspension;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Cirrious.MvvmCross.WinRT.Views
{
    public abstract class MvxWinRTPage
        : Page
          , IMvxWinRTView
    {
        private IMvxViewModel _viewModel;

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


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var bundle = LoadStateBundle(e);

            this.OnViewCreate(e.Parameter as MvxShowViewModelRequest, bundle);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            var bundle = this.CreateSaveStateBundle();
            SaveStateBundle(e, bundle);
            
            if (e.NavigationMode == NavigationMode.Back)
                this.OnViewDestroy();
        }

        private String _pageKey;

        private IMvxSuspensionManager _suspensionManager;
        protected IMvxSuspensionManager SuspensionManager
        {
            get
            {
                _suspensionManager = _suspensionManager ?? Mvx.Resolve<IMvxSuspensionManager>();
                return _suspensionManager;
            }
        }

        protected virtual IMvxBundle LoadStateBundle(NavigationEventArgs e)
        {
            // nothing loaded by default
            var frameState = SuspensionManager.SessionStateForFrame(this.Frame);
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
                var dictionary = (IDictionary<string, string>)frameState[this._pageKey];
                bundle = new MvxBundle(dictionary);
            }

            return bundle;
        }

        protected virtual void SaveStateBundle(NavigationEventArgs navigationEventArgs, IMvxBundle bundle)
        {
            var frameState = SuspensionManager.SessionStateForFrame(this.Frame);
            frameState[_pageKey] = bundle.Data;
        }
    }
}