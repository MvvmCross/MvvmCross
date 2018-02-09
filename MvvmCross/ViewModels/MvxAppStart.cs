// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Base.Exceptions;
using MvvmCross.Base.Logging;
using MvvmCross.Core.Navigation;

namespace MvvmCross.Core.ViewModels
{
    public class MvxAppStart<TViewModel>
        : IMvxAppStart
        where TViewModel : IMvxViewModel
    {
        protected readonly IMvxNavigationService NavigationService;

        public MvxAppStart(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public async void Start(object hint = null)
        {
            if (hint != null) {
                MvxLog.Instance.Trace("Hint ignored in default MvxAppStart");
            }
            try {
                await NavigationService.Navigate<TViewModel>();
            } catch (System.Exception exception) {
                throw exception.MvxWrap("Problem navigating to ViewModel {0}", typeof(TViewModel).Name);
            }
        }
    }
}
