using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.WindowsCommon.Views.Suspension;

namespace Cirrious.MvvmCross.WindowsCommon.Views.Handlers
{
    internal class PageStateHandler
    {
        private readonly MtPage _page;

        public string PageKey { get; private set; }

        internal event EventHandler<MvxWindowsLoadStateEventArgs> LoadState;
        internal event EventHandler<MvxWindowsSaveStateEventArgs> SaveState;

        private IMvxSuspensionManager _suspensionManager;
        protected IMvxSuspensionManager SuspensionManager
        {
            get
            {
                _suspensionManager = _suspensionManager ?? Mvx.Resolve<IMvxSuspensionManager>();
                return _suspensionManager;
            }
        }

        public PageStateHandler(MtPage page)
        {
            _page = page;
        }

        public void OnNavigatedTo(MvxWindowsNavigationEventArgs e)
        {
            if (PageKey != null) // new instance
                return;

            var frameState = SuspensionManager.SessionStateForFrame(_page.Frame);
            PageKey = "Page" + _page.Frame.BackStackDepth;

            if (e.NavigationMode == NavigationMode.New)
            {
                var nextPageKey = PageKey;
                var nextPageIndex = _page.Frame.BackStackDepth;
                while (frameState.Remove(nextPageKey))
                {
                    nextPageIndex++;
                    nextPageKey = "Page" + nextPageIndex;
                }

                // Does not make sense when no page state is available...
                //var args = new MtLoadStateEventArgs(e.Parameter, null);

                //var copy = LoadState;
                //if (copy != null)
                //    copy(this, args);

                _page.LoadState(e.Parameter, null);
                //_page.OnLoadState(args);
            }
            else
            {
                var pageState = (Dictionary<String, Object>)frameState[PageKey];
                var args = new MvxWindowsLoadStateEventArgs(e.Parameter, pageState);

                var copy = LoadState;
                if (copy != null)
                    copy(this, args);

                _page.LoadState(e.Parameter, pageState);
                _page.OnLoadState(args);
            }
        }

        public void OnNavigatedFrom(MvxWindowsNavigationEventArgs e)
        {
            var frameState = SuspensionManager.SessionStateForFrame(_page.Frame);
            var pageState = new Dictionary<String, Object>();
            var args = new MvxWindowsSaveStateEventArgs(pageState);

            var copy = SaveState;
            if (copy != null)
                copy(this, args);

            _page.SaveState(pageState);
            _page.OnSaveState(args);
            frameState[PageKey] = pageState;
        }
    }
}
