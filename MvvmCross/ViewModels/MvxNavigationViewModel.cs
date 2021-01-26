// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MvvmCross.ViewModels
{
#nullable enable
    public abstract class MvxNavigationViewModel
        : MvxViewModel
    {
        private IMvxLog? _log;

        protected MvxNavigationViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
        {
            LogProvider = logProvider;
            NavigationService = navigationService;
        }

        protected virtual IMvxNavigationService NavigationService { get; }

        protected virtual IMvxLogProvider LogProvider { get; }

        protected virtual IMvxLog Log => _log ??= LogProvider.GetLogFor(GetType());
    }

    public abstract class MvxNavigationViewModel<TParameter> : MvxNavigationViewModel, IMvxViewModel<TParameter>
        where TParameter : class
    {
        protected MvxNavigationViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }

        public abstract void Prepare(TParameter parameter);
    }

    public abstract class MvxNavigationViewModelResult<TResult> : MvxNavigationViewModel, IMvxViewModelResult<TResult>
        where TResult : class
    {
        protected MvxNavigationViewModelResult(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }

        public TaskCompletionSource<object>? CloseCompletionSource { get; set; }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing && CloseCompletionSource != null &&
                !CloseCompletionSource.Task.IsCompleted &&
                !CloseCompletionSource.Task.IsFaulted)
            {
                CloseCompletionSource.TrySetCanceled();
            }

            base.ViewDestroy(viewFinishing);
        }
    }

    public abstract class MvxNavigationViewModel<TParameter, TResult> : MvxNavigationViewModelResult<TResult>, IMvxViewModel<TParameter, TResult>
        where TParameter : class
        where TResult : class
    {
        protected MvxNavigationViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }

        public abstract void Prepare(TParameter parameter);
    }
#nullable restore
}
