using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Navigation;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Silverlight.Views
{
    public abstract class MvxSilverlightPage : Page, IMvxSilverlightView
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
            //TODO Mvx - ClearBackStack
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var savedState = LoadStateBundle(e);
            this.OnViewCreate(e.Uri, savedState);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var bundle = this.CreateSaveStateBundle();
            SaveStateBundle(e, bundle);

            base.OnNavigatedFrom(e);
        }

        private String _pageKey;

        protected virtual IMvxBundle LoadStateBundle(NavigationEventArgs e)
        {

            // nothing loaded by default
            //TODO Mvx - var frameState = SuspensionManager.SessionStateForFrame(this.Frame);
            _pageKey = "Page-" + "x";//TODO Mvx - this.Frame.BackStackDepth;
            IMvxBundle bundle = null;

            //TODO Mvx - if (e.NavigationMode == NavigationMode.New)
            {
                // Clear existing state for forward navigation when adding a new page to the
                // navigation stack
                /* TODO Mvx - LoadStateBundle
                 var nextPageKey = this._pageKey;
                int nextPageIndex = this.Frame.BackStackDepth;
                while (frameState.Remove(nextPageKey))
                {
                    nextPageIndex++;
                    nextPageKey = "Page-" + nextPageIndex;
                }*/
            }
            //TODO Mvx - else
            {
                var dictionary = (IDictionary<string, string>)new Dictionary<string, string>();//TODO Mvx - frameState[this._pageKey];
                bundle = new MvxBundle(dictionary);
            }

            return bundle;/**/
        }

        protected virtual void SaveStateBundle(NavigationEventArgs navigationEventArgs, IMvxBundle bundle)
        {
            //TODO Mvx - SaveStateBundle

            /*
            var frameState = SuspensionManager.SessionStateForFrame(this.Frame);
            frameState[_pageKey] = bundle.Data;*/
        }
    }
}