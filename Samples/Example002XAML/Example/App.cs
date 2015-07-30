using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using System.Linq;

namespace Example
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
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