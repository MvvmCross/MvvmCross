// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;
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

            Startup(hint).GetAwaiter().GetResult();
        }

        public async Task StartAsync(object hint = null)
        {
            // Check whether Start has commenced, and return if it has
            if (Interlocked.CompareExchange(ref startHasCommenced, 1, 0) == 1)
                return;

            await Startup(hint);
        }

        protected virtual async Task Startup(object hint = null)
        {
            await ApplicationStartup(hint);
        }

        protected virtual async Task ApplicationStartup(object hint = null)
        {
            MvxLog.Instance.Trace("AppStart: Application Startup - On UI thread");
            await Application.Startup(hint);
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

        protected override async Task Startup(object hint = null)
        {
            await base.Startup(hint);

            await NavigateToFirstViewModel(hint);
        }

        protected virtual async Task NavigateToFirstViewModel(object hint)
        {
            if (hint != null)
            {
                MvxLog.Instance.Trace("Hint ignored in default MvxAppStart");
            }

            try
            {
                await NavigationService.Navigate<TViewModel>();
            }
            catch (System.Exception exception)
            {
                throw exception.MvxWrap("Problem navigating to ViewModel {0}", typeof(TViewModel).Name);
            }
        }
    }
}
