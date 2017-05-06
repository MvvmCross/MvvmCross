namespace $rootnamespace$

using MvvmCross.Core.ViewModels
using MvvmCross.iOS.Platform
using MvvmCross.iOS.Views.Presenters
using MvvmCross.Platform.Platform
using UIKit

type Setup(applicationDelegate: MvxApplicationDelegate, window: UIWindow) =
    inherit MvxIosSetup(applicationDelegate, window)

    override this.CreateApp() = upcast new Core.App()
    override this.CreateDebugTrace() = upcast new DebugTrace()