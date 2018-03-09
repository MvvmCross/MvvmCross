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
    public class MvxAppStart<TViewModel>
        : IMvxAppStart
        where TViewModel : IMvxViewModel
    {
        protected readonly IMvxNavigationService NavigationService;

        private SemaphoreSlim StartWaiter { get; set; } = new SemaphoreSlim(1);

        private bool StartHasBeenRun { get; set; }

        public MvxAppStart(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public async void Start(object hint = null)
        {
            if (StartHasBeenRun) return;
            StartHasBeenRun = true;

            if (hint != null) {
                MvxLog.Instance.Trace("Hint ignored in default MvxAppStart");
            }
            StartWaiter.Wait();
            try {
                await NavigationService.Navigate<TViewModel>();
            } catch (System.Exception exception) {
                throw exception.MvxWrap("Problem navigating to ViewModel {0}", typeof(TViewModel).Name);
            } finally {
                StartWaiter.Release();
            }
        }

        public async Task WaitForStart()
        {
            await StartWaiter.WaitAsync();
            StartWaiter.Release();
        }
    }
}
