using MvvmCross.IoC;
using MvvmCross.ViewModels;
using $rootnamespace$.ViewModels;

namespace $rootnamespace$
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<HomeViewModel>();
        }
    }
}
