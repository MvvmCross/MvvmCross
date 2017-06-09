using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.IoC;

namespace MvvmCross.iOS.Support.XamarinSidebarSample.Core
{
    public class App : MvxApplication
    {
        /// <summary>The initialize.</summary>
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart(new AppStart());
        }
    }
}