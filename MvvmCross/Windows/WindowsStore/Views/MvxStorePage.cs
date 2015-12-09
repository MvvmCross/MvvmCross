// MvxWinRTPage.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.WindowsStore.Views
{
    using System;
    using System.Collections.Generic;

    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform;
    using MvvmCross.WindowsStore.Views.Suspension;

    public class MvxStorePage
        : Page
          , IMvxStoreView
    {
        private IMvxViewModel _viewModel;

        public IMvxViewModel ViewModel
        {
            get { return this._viewModel; }
            set
            {
                if (this._viewModel == value)
                    return;

                this._viewModel = value;
                this.DataContext = this.ViewModel;
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

            var reqData = (string)e.Parameter;
            var converter = Mvx.Resolve<IMvxNavigationSerializer>();
            var req = converter.Serializer.DeserializeObject<MvxViewModelRequest>(reqData);

            this.OnViewCreate(req, () => this.LoadStateBundle(e));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            var bundle = this.CreateSaveStateBundle();
            this.SaveStateBundle(e, bundle);

            if (e.NavigationMode == NavigationMode.Back)
                this.OnViewDestroy();
        }

        private String _pageKey;

        private IMvxSuspensionManager _suspensionManager;

        protected IMvxSuspensionManager SuspensionManager
        {
            get
            {
                this._suspensionManager = this._suspensionManager ?? Mvx.Resolve<IMvxSuspensionManager>();
                return this._suspensionManager;
            }
        }

        protected virtual IMvxBundle LoadStateBundle(NavigationEventArgs e)
        {
            // nothing loaded by default
            var frameState = this.SuspensionManager.SessionStateForFrame(this.Frame);
            this._pageKey = "Page-" + this.Frame.BackStackDepth;
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
            var frameState = this.SuspensionManager.SessionStateForFrame(this.Frame);
            frameState[this._pageKey] = bundle.Data;
        }
    }

    public class MvxStorePage<TViewModel>
        : MvxStorePage where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}