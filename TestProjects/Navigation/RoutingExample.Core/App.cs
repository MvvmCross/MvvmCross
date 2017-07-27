using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using RoutingExample.Core.ViewModels;

namespace RoutingExample.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            // register the appstart object
            RegisterNavigationServiceAppStart<MainViewModel>();
        }
    }
}
