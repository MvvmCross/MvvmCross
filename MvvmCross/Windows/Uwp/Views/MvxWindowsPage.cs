// MvxWinRTPage.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Uwp.Views.Suspension;

namespace MvvmCross.Uwp.Views
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
            ViewModel?.ViewDisappeared();
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

        public void ClearBackStack()
        {
            throw new NotImplementedException();
            /*
            // note - we do *not* use CanGoBack here - as that seems to always returns true!
            while (NavigationService.BackStack.Any())
                NavigationService.RemoveBackEntry();
         */
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