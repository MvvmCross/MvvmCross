using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using Playground.Core.Services;
using Playground.Core.ViewModels;

namespace Playground.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            var test = new TextProviderBuilder();
            Mvx.RegisterSingleton<IMvxTextProvider>(test.TextProvider);

            RegisterNavigationServiceAppStart<RootViewModel>();
        }
    }
}
