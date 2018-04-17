using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.IoC;
using MvvmCross.TestProjects.CustomBinding.Core.ViewModels;

namespace MvvmCross.TestProjects.CustomBinding.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<FirstViewModel>();
        }
    }
}
