namespace $rootnamespace$

open $rootnamespace$.ViewModels
open MvvmCross.Platform.IoC
open MvvmCross.Core.ViewModels

type App() =
    inherit MvxApplication()

    override this.Initialize() =
        this.CreatableTypes()
            .EndingWith("Service")
            .AsInterfaces()
            .RegisterAsLazySingleton();

        this.RegisterAppStart<MainViewModel>()