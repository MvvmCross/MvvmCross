// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MvvmCross.ViewModels
{
    public abstract class MvxAppStart : IMvxAppStart
    {
        private int startHasCommenced;

        public void Start(object hint = null)
        {
            // Check whether Start has commenced, and return if it has
            if (Interlocked.CompareExchange(ref startHasCommenced, 1, 0) == 1)
                return;

            Startup(hint);
        }

        protected abstract void Startup(object hint = null);

        public virtual bool IsStarted => startHasCommenced != 0;

        public virtual void ResetStart()
        {
            Reset();
            Interlocked.Exchange(ref startHasCommenced, 0);
        }

        protected virtual void Reset()
        { }
    }

    public class MvxAppStart<TViewModel> : MvxAppStart
        where TViewModel : IMvxViewModel
    {
        protected readonly IMvxNavigationService NavigationService;
        protected readonly IMvxApplication Application;

        public MvxAppStart(IMvxNavigationService navigationService, IMvxApplication application)
        {
            NavigationService = navigationService;
            Application = application;
        }

        protected override void Startup(object hint = null)
        {
            ApplicationStartup();

            NavigateToFirstViewModel(hint);
        }

        protected virtual void ApplicationStartup(object hint = null)
        {
            MvxLog.Instance.Trace("AppStart: Application Startup - On UI thread");
            Application.Startup(hint);
        }

        protected virtual void NavigateToFirstViewModel(object hint)
        {
            if (hint != null)
            {
                MvxLog.Instance.Trace("Hint ignored in default MvxAppStart");
            }

            try
            {
                NavigationService.Navigate<TViewModel>().GetAwaiter().GetResult();
            }
            catch (System.Exception exception)
            {
                throw exception.MvxWrap("Problem navigating to ViewModel {0}", typeof(TViewModel).Name);
            }
        }

        protected override void Reset()
        {
            Application.Reset();
            base.Reset();
        }
    }
}
