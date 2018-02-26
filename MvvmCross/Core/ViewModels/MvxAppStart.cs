// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Logging;

namespace MvvmCross.Core.ViewModels
{
    public class MvxAppStart<TViewModel> : IMvxAppStart where TViewModel : IMvxViewModel
    {
        protected readonly IMvxNavigationService NavigationService;

        public MvxAppStart(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public async void Start(object hint = null)
        {
            try
            {
                await StartAsync(hint);
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap("Problem navigating to ViewModel {0}", typeof(TViewModel).Name);
            }
        }

        public virtual Task StartAsync(object hint = null)
        {
            if (hint != null)
            {
                MvxLog.Instance.Trace("Hint ignored in default MvxAppStart");
            }

            return NavigationService.Navigate<TViewModel>();
        }
    }
}
