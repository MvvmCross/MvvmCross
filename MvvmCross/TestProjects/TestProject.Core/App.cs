using MvvmCross.Core.ViewModels;

namespace TestProject.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<ViewModels.ContentViewModel>();
        }
    }
}
