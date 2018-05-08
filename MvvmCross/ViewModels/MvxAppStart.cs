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

        public void Start(object hint = null)
        {
            // Check whether Start has commenced, and return if it has
            if (Interlocked.CompareExchange(ref startHasCommenced, 1, 0) == 1)
                return;

            Startup(hint);
        }

        protected virtual object Startup(object hint = null)
        {
            return ApplicationStartup(hint);
        }

        protected virtual object ApplicationStartup(object hint = null)
        {
            MvxLog.Instance.Trace("AppStart: Application Startup - On UI thread");

            if (hint != null)
            {
                MvxLog.Instance.Trace("Use generic MvxAppStart so that hint can be passed to Application.StartupWithHint");
            }

            Application.Startup();

            return hint;
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

    public class MvxAppStart<TViewModel, TParameter> : MvxAppStart<TViewModel> where TViewModel : IMvxViewModel
    {
        public MvxAppStart(IMvxApplication application, IMvxNavigationService navigationService) : base(application, navigationService)
        {
        }

        protected override object ApplicationStartup(object hint = null)
        {
            if (hint is TParameter typedHint &&
                Application is IMvxApplication<TParameter> typedApplication)
            {
                return typedApplication.StartupWithHint(typedHint);
            }

            return base.ApplicationStartup(hint);
        }

        protected override void NavigateToFirstViewModel(object hint)
        {
            if (hint != null)
            {
                MvxLog.Instance.Trace("Native platform hint ignored in default MvxAppStart");
            }

            if (hint is TParameter typedHint)
            {
                navParam = typedHint;
            }

            try
            {
                if (typeof(IMvxViewModel<TParameter>).IsAssignableFrom(typeof(TViewModel)))
                {
                    NavigationService.Navigate(typeof(TViewModel), navParam).GetAwaiter().GetResult();
                }
                else
                {
                    NavigationService.Navigate<TViewModel>().GetAwaiter().GetResult();
                }
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

        protected override object ApplicationStartup(object hint = null)
        {
            if (hint is TParameter typedHint &&
                Application is IMvxApplication<TParameter> typedApplication)
            {
                return typedApplication.StartupWithHint(typedHint);
            }

            return base.ApplicationStartup(hint);
        }

        protected override void NavigateToFirstViewModel(object hint)
        {
            TParameter navParam = default;

            if (hint is TParameter typedHint)
            {
                navParam = typedHint;
            }

            try
            {
                if (typeof(IMvxViewModel<TParameter>).IsAssignableFrom(typeof(TViewModel)))
                {
                    NavigationService.Navigate(typeof(TViewModel), navParam).GetAwaiter().GetResult();
                }
                else
                {
                    NavigationService.Navigate<TViewModel>().GetAwaiter().GetResult();
                }
            }
            catch (System.Exception exception)
            {
                throw exception.MvxWrap("Problem navigating to ViewModel {0}", typeof(TViewModel).Name);
            }
        }
    }
}
