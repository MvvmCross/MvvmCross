// MvxAppStart.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Core.Navigation;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Logging;

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
