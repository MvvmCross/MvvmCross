// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Platforms.Uap.Views.Suspension;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MvvmCross.Platforms.Uap.Views
{
    public class MvxWindowsPage
        : Page
        , IMvxWindowsView
        , IDisposable
    {
        public MvxWindowsPage()
        {
            Loading += MvxWindowsPage_Loading;
            Loaded += MvxWindowsPage_Loaded;
            Unloaded += MvxWindowsPage_Unloaded;
        }

        private void MvxWindowsPage_Loading(FrameworkElement sender, object args)
        {
            ViewModel?.ViewAppearing();
        }

        private void MvxWindowsPage_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.ViewAppeared();
        }

        private void MvxWindowsPage_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.ViewDestroy();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewModel?.ViewDisappearing();
            base.OnNavigatingFrom(e);
        }

        private IMvxViewModel _viewModel;

        public IMvxWindowsFrame WrappedFrame => new MvxWrappedFrame(Frame);

        public IMvxViewModel ViewModel
        {
            get
            {
                return _viewModel;
            }
            set
            {
                if (_viewModel == value)
                    return;

                _viewModel = value;
                DataContext = ViewModel;
            }
        }

        public virtual void ClearBackStack()
        {
            var backStack = base.Frame?.BackStack;

            while (backStack != null && backStack.Any())
            {
                backStack.RemoveAt(0);
            }

            UpdateBackButtonVisibility();
        }
        
        protected virtual void UpdateBackButtonVisibility()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private string _reqData = string.Empty;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel?.ViewCreated();

            if (_reqData != string.Empty)
            {
                var viewModelLoader = Mvx.Resolve<IMvxWindowsViewModelLoader>();
                ViewModel = viewModelLoader.Load(e.Parameter.ToString(), LoadStateBundle(e));
            }
            _reqData = (string)e.Parameter;

            this.OnViewCreate(_reqData, () => LoadStateBundle(e));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel?.ViewDisappeared();
            base.OnNavigatedFrom(e);
            var bundle = this.CreateSaveStateBundle();
            SaveStateBundle(e, bundle);

            var translator = Mvx.Resolve<IMvxWindowsViewModelRequestTranslator>();

            if (e.NavigationMode == NavigationMode.Back)
            {
                var key = translator.RequestTextGetKey(_reqData);
                this.OnViewDestroy(key);
            }
            else
            {
                var backstack = Frame.BackStack;
                if (backstack.Count > 0)
                {
                    var currentEntry = backstack[backstack.Count - 1];
                    var key = translator.RequestTextGetKey(currentEntry.Parameter.ToString());
                    if (key == 0)
                    {
                        var newParamter = translator.GetRequestTextWithKeyFor(ViewModel);
                        var entry = new PageStackEntry(currentEntry.SourcePageType, newParamter, currentEntry.NavigationTransitionInfo);
                        backstack.Remove(currentEntry);
                        backstack.Add(entry);
                    }
                }
            }
        }

        private string _pageKey;

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
            var frameState = SuspensionManager.SessionStateForFrame(WrappedFrame);
            _pageKey = "Page-" + Frame.BackStackDepth;
             IMvxBundle bundle = null;

            if (e.NavigationMode == NavigationMode.New)
            {
                // Clear existing state for forward navigation when adding a new page to the
                // navigation stack
                var nextPageKey = _pageKey;
                var nextPageIndex = Frame.BackStackDepth;
                while (frameState.Remove(nextPageKey))
                {
                    nextPageIndex++;
                    nextPageKey = "Page-" + nextPageIndex;
                }
            }
            else
            {
                var dictionary = (IDictionary<string, string>)frameState[_pageKey];
                bundle = new MvxBundle(dictionary);
            }

            return bundle;
        }

        protected virtual void SaveStateBundle(NavigationEventArgs navigationEventArgs, IMvxBundle bundle)
        {
            var frameState = SuspensionManager.SessionStateForFrame(WrappedFrame);
            frameState[_pageKey] = bundle.Data;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MvxWindowsPage()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Loading -= MvxWindowsPage_Loading;
                Loaded -= MvxWindowsPage_Loaded;
                Unloaded -= MvxWindowsPage_Unloaded;
            }
        }
    }

    public class MvxWindowsPage<TViewModel>
        : MvxWindowsPage
        , IMvxWindowsView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}
