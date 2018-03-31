using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace $rootnamespace$.Core
{
    public class CoreApp : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterNavigationServiceAppStart<ViewModels.MainViewModel>();
        }
    }
}
