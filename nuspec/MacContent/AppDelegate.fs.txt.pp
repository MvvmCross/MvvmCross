namespace $rootnamespace$

open System
open AppKit
open CoreGraphics
open Foundation
open ObjCRuntime
open MvvmCross.Mac.Views.Presenters
open MvvmCross.Platform
open MvvmCross.Core.ViewModels
open MvvmCross.Mac.Platform

[<Register ("AppDelegate")>]
type AppDelegate() =
    inherit MvxApplicationDelegate()

    let mutable window : NSWindow = null

    override this.DidFinishLaunching(notification: NSNotification) =

        window <- new NSWindow(new CGRect(200, 200, 400, 700), NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled,
        NSBackingStore.Buffered, false, NSScreen.MainScreen)

        var setup = new Setup(this, window)
        setup.Initialize()

        var startup = Mvx.Resolve<IMvxAppStart>()
        startup.Start()

        window.MakeKeyAndOrderFront(this)