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
        protected readonly IMvxApplication Application;

        private int startHasCommenced;

        public MvxAppStart(IMvxApplication application)
        {
            Application = application;
        }

        public object Start(object hint = null)
        {
            // Check whether Start has commenced, and return if it has
            if (Interlocked.CompareExchange(ref startHasCommenced, 1, 0) == 1)
                return hint;

            return Startup(hint);
        }

        protected virtual object Startup(object hint = null)
        {
            return ApplicationStartup(hint);
        }

        protected virtual object ApplicationStartup(object hint = null)
        {
            MvxLog.Instance.Trace("AppStart: Application Startup - On UI thread");
            return Application.Startup(hint);
        }

        public virtual bool IsStarted => startHasCommenced != 0;

        public virtual void ResetStart()
        {
            Reset();
            Interlocked.Exchange(ref startHasCommenced, 0);
        }

        protected virtual void Reset()
        {
            Application.Reset();
        }
    }

    public class MvxAppStart<TViewModel> : MvxAppStart
        where TViewModel : IMvxViewModel
    {
        protected readonly IMvxNavigationService NavigationService;

        public MvxAppStart(IMvxApplication application, IMvxNavigationService navigationService) : base(application)
        {
            NavigationService = navigationService;
        }

        protected override object Startup(object hint = null)
        {
            hint = base.Startup(hint);

            NavigateToFirstViewModel(hint);

            return hint;
        }

        protected virtual void NavigateToFirstViewModel(object hint)
        {
            if (hint != null)
            {
                MvxLog.Instance.Trace("Native platform hint ignored in default MvxAppStart");
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
    }

    public class MvxAppStart<TViewModel, TParameter> : MvxAppStart<TViewModel> where TViewModel : IMvxViewModel<TParameter>
    {
        public MvxAppStart(IMvxApplication application, IMvxNavigationService navigationService) : base(application, navigationService)
        {
        }

        protected override void NavigateToFirstViewModel(object hint)
        {
            if (hint != null)
            {
                MvxLog.Instance.Trace("Native platform hint ignored in default MvxAppStart");
            }

            if (hint is TParameter)
            {
                navParam = startHint;
            }

            try
            {
                NavigationService.Navigate<TViewModel, TParameter>(navParam).GetAwaiter().GetResult();
            }
            catch (System.Exception exception)
            {
                throw exception.MvxWrap("Problem navigating to ViewModel {0}", typeof(TViewModel).Name);
            }
        }
    }
}
