namespace $rootnamespace$

open Android.Content
open MvvmCross.Droid.Platform
open MvvmCross.Core.ViewModels
open MvvmCross.Platform.Platform

type Setup(applicationContext: Context) =
    inherit MvxAndroidSetup(applicationContext)

    override __.CreateApp() = upcast new Core.App()

    override  __.CreateDebugTrace() = upcast new DebugTrace()