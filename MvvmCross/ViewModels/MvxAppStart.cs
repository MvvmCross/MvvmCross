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
        : IMvxAppStart, IMvxAppStartAsync
        where TViewModel : IMvxViewModel
    {
        protected readonly IMvxNavigationService NavigationService;

        private int startHasCommenced;

        public MvxAppStart(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }



        public void Start(object hint = null)
        {
            // Check whether Start has commenced, and return if it has
            if (Interlocked.CompareExchange(ref startHasCommenced, 1, 0) == 1) return;

            if (hint != null) {
                MvxLog.Instance.Trace("Hint ignored in default MvxAppStart");
            }
            
            try {
                _startTaskNotifier = MvxNotifyTask.Create(async ()=> {
                    await NavigationService.Navigate<TViewModel>();
                    await Task.Delay(10000);  // TODO: remove, this is for testing only 
                });
            } catch (System.Exception exception) {
                throw exception.MvxWrap("Problem navigating to ViewModel {0}", typeof(TViewModel).Name);
            } 
        }

        public bool IsStarted => startHasCommenced != 0;

        private MvxNotifyTask _startTaskNotifier;

        public async Task<bool> WaitForStart()
        {
            if (_startTaskNotifier != null) {
                await _startTaskNotifier.Task;
                return true;
            }
            return false;
        }
    }
}
