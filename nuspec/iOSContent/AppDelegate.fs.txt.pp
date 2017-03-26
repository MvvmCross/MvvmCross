namespace $rootnamespace$

open MvvmCross.Core.ViewModels
open MvvmCross.iOS.Platform
open MvvmCross.Platform
open Foundation
open UIKit

[<Register ("AppDelegate")>]
type AppDelegate () =
    inherit MvxApplicationDelegate()

    override val Window = null with get, set

    override this.FinishedLaunching (app, options) =

        this.Window <- new UIWindow(UIScreen.MainScreen.Bounds)

        let setup = new Setup(this, this.Window)
        setup.Initialize()

        let startup = Mvx.Resolve<IMvxAppStart>()
        startup.Start()

        this.Window.MakeKeyAndVisible()
        true