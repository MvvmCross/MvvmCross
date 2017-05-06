namespace $rootnamespace$

open AppKit
open Foundation
open MvvmCross.Core.ViewModels
open MvvmCross.Mac.Platform
open MvvmCross.Mac.Views.Presenters
open MvvmCross.Platform.Platform


type Setup(applicationDelegate: MvxApplicationDelegate, window: NSWindow) =
    inherit MvxMacSetup(applicationDelegate, window)

    override this.CreateApp() = upcast new Core.App()

    override this.CreateDebugTrace() = upcast new DebugTrace()