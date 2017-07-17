using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Navigation;
using MvvmCross.Platform;
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

            Mvx.ConstructAndRegisterSingleton<IMvxAppStart, AppStart>();
            var appStart = Mvx.Resolve<IMvxAppStart>();

            // register the appstart object
            RegisterAppStart(appStart);
        }
    }
}