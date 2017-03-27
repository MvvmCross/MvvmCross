namespace $rootnamespace$.Views

open System
open System.Collections.Generic
open System.Linq
open AppKit
open Foundation
open MvvmCross.Mac.Views
open MvvmCross.Binding.BindingContext
open MvvmCross.Core.ViewModels

[<MvxViewFor(typeof<Core.ViewModels.MainViewModel>)>]
[<Export("initWithCoder:")>]
type MainViewController(coder: NSCoder) =
    inherit MvxViewController(coder)

    override this.ViewDidLoad() = 

        base.ViewDidLoad ()

        let set = this.CreateBindingSet<MainViewController, Core.ViewModels.MainViewModel>()
        //set.Bind(textMain).To(vm => vm.Hello)
        set.Apply()