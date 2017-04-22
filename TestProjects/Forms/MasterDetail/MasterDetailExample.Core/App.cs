using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.IoC;
using Xamarin.Forms;
using MvvmCross.Platform;

namespace MasterDetailExample.Core
{
    public class App : MvxApplication
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
