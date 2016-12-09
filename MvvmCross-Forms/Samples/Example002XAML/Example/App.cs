using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using System.Linq;

namespace Example
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
				
            RegisterAppStart<ViewModels.MainViewModel>();
        }
    }
}