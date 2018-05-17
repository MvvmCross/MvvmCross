using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Playground.Droid.ViewModels;

namespace Playground.Droid
{
    public class AppStart : MvxAppStart
    {
        private readonly IMvxNavigationService _mvxNavigationService;

        public AppStart(IMvxApplication app, IMvxNavigationService mvxNavigationService)
            : base(app)
        {
            _mvxNavigationService = mvxNavigationService;
        }

        protected override void Startup(object hint = null)
        {
            _mvxNavigationService.Navigate<MainViewModel>();
        }
    }
}
