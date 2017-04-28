using MvvmCross.Platform.IoC;
using MvxBindingsExample.ViewModels;

namespace MvxBindingsExample
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
				
            RegisterAppStart<MainViewModel>();
        }
    }
}