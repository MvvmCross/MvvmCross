using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.IoC;
using TipCalc.Core.ViewModels;

namespace TipCalc.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterNavigationServiceAppStart<TipViewModel>();
        }


    }
}
