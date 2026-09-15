// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MvvmCross.ViewModels
{
    public abstract class MvxAppStart : IMvxAppStart
    {
        protected readonly IMvxNavigationService NavigationService;

        private int startHasCommenced;

        protected MvxAppStart(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        [RequiresUnreferencedCode("Navigation uses presentation attributes and view type lookups that may not be preserved during trimming.")]
        public void Start(object? hint = null)
        {
            StartAsync(hint).GetAwaiter().GetResult();
        }

        [RequiresUnreferencedCode("Navigation uses presentation attributes and view type lookups that may not be preserved during trimming.")]
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

        [RequiresUnreferencedCode("Navigation uses presentation attributes and view type lookups that may not be preserved during trimming.")]
        protected abstract Task NavigateToFirstViewModel(object? hint = null);

        protected virtual Task<object?> ApplicationStartup(object? hint = null)
        {
            return Task.FromResult(hint);
        }

        public virtual bool IsStarted => startHasCommenced != 0;

        public virtual void ResetStart()
        {
            Reset();
            Interlocked.Exchange(ref startHasCommenced, 0);
        }

        protected virtual void Reset()
        {
            // override to handle app restart
        }
    }

    public class MvxAppStart<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>
        : MvxAppStart
            where TViewModel : IMvxViewModel
    {
        public MvxAppStart(IMvxNavigationService navigationService)
            : base(navigationService)
        {
        }

        [RequiresUnreferencedCode("Navigation uses presentation attributes and view type lookups that may not be preserved during trimming.")]
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

    public class MvxAppStart<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel, TParameter>
        : MvxAppStart<TViewModel>
            where TViewModel : IMvxViewModel<TParameter>
            where TParameter : notnull
    {
        public MvxAppStart(IMvxNavigationService navigationService)
            : base(navigationService)
        {
        }

        protected override async Task<object?> ApplicationStartup(object? hint = null)
        {
            return await base.ApplicationStartup(hint);
        }

        [RequiresUnreferencedCode("Navigation uses presentation attributes and view type lookups that may not be preserved during trimming.")]
        protected override async Task NavigateToFirstViewModel(object? hint = null)
        {
            try
            {
                if (hint is TParameter parameter)
                    NavigationService.Navigate<TViewModel, TParameter>(parameter).GetAwaiter().GetResult();
                else
                {
                    MvxLogHost.Default?.Log(
                        LogLevel.Information,
                        "Hint is not matching type of {ParameterName}. Doing navigation without typed parameter instead",
                        nameof(TParameter));
                    await base.NavigateToFirstViewModel(hint);
                }
            }
            catch (System.Exception exception)
            {
                throw exception.MvxWrap("Problem navigating to ViewModel {0}", typeof(TViewModel).Name);
            }
        }
    }
}
