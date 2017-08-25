using MvvmCross.Core.ViewModels;

namespace RoutingExample.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            // register the generic appstart object and navigate to View Model with navigation service
            //RegisterNavigationServiceAppStart<MainViewModel>();

            // register the custom appstart object 
            RegisterCustomAppStart<AppStart>();
        }
    }
}
