// MvxAppStart.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Core.Navigation;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Logging;

namespace MvvmCross.Core.ViewModels
{
    public class MvxNavigationServiceAppStart<TViewModel>
        : IMvxAppStart
        where TViewModel : IMvxViewModel
    {
        protected readonly IMvxNavigationService NavigationService;

        public MvxNavigationServiceAppStart(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public void Start(object hint = null)
        {
            if (hint != null)
            {
                MvxSingleton<IMvxLog>.Instance.Trace("Hint ignored in default MvxAppStart");
            }
            try
            {
                NavigationService.Navigate<TViewModel>();
            }
            catch (System.Exception exception)
            {
                throw exception.MvxWrap("Problem navigating to ViewModel {0}", typeof(TViewModel).Name);
            }
        }
    }
}