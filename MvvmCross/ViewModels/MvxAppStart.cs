// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MvvmCross.ViewModels
{
#nullable enable
    public abstract class MvxAppStart : IMvxAppStart
    {
        protected readonly IMvxNavigationService NavigationService;
        protected readonly IMvxApplication Application;

        private int startHasCommenced;

        protected MvxAppStart(IMvxApplication application, IMvxNavigationService navigationService)
        {
            Application = application;
            NavigationService = navigationService;
        }

        public void Start(object? hint = null)
        {
            StartAsync(hint).GetAwaiter().GetResult();
        }

        public async Task StartAsync(object? hint = null)
        {
            // Check whether Start has commenced, and return if it has
            if (Interlocked.CompareExchange(ref startHasCommenced, 1, 0) == 1)
                return;

            var applicationHint = await ApplicationStartup(hint);
            if (applicationHint != null)
            {
                MvxLogHost.Default?.Log(LogLevel.Trace, "Hint ignored in default MvxAppStart");
            }

            await NavigateToFirstViewModel(applicationHint);
        }

        protected abstract Task NavigateToFirstViewModel(object? hint = null);

        protected virtual async Task<object?> ApplicationStartup(object? hint = null)
        {
            await Application.Startup();
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
        public MvxAppStart(IMvxApplication application, IMvxNavigationService navigationService) : base(application, navigationService)
        {
        }

        protected override async Task NavigateToFirstViewModel(object? hint = null)
        {
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

    public class MvxAppStart<TViewModel, TParameter> : MvxAppStart<TViewModel>
        where TViewModel : IMvxViewModel<TParameter>
        where TParameter : notnull
    {
        public MvxAppStart(IMvxApplication application, IMvxNavigationService navigationService) : base(application, navigationService)
        {
        }

        protected override async Task<object?> ApplicationStartup(object? hint = null)
        {
            var applicationHint = await base.ApplicationStartup(hint);
            if (applicationHint is TParameter parameter && Application is IMvxApplication<TParameter> typedApplication)
                return typedApplication.Startup(parameter);
            else
                return applicationHint;
        }

        protected override async Task NavigateToFirstViewModel(object? hint = null)
        {
            try
            {
                if (hint is TParameter parameter)
                    NavigationService.Navigate<TViewModel, TParameter>(parameter).GetAwaiter().GetResult();
                else
                {
                    MvxLogHost.Default?.Log(LogLevel.Trace, "Hint is not matching type of {parameterName}. Doing navigation without typed parameter instead.", nameof(TParameter));
                    await base.NavigateToFirstViewModel(hint);
                }
            }
            catch (System.Exception exception)
            {
                throw exception.MvxWrap("Problem navigating to ViewModel {0}", typeof(TViewModel).Name);
            }
        }
    }
#nullable restore
}
