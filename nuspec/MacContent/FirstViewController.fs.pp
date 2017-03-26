namespace $rootnamespace$.Views

open System
open System.Collections.Generic
open System.Linq
open AppKit
open Foundation
open MvvmCross.Mac.Views
open MvvmCross.Binding.BindingContext
open MvvmCross.Core.ViewModels

[<MvxViewFor(typeof<Core.ViewModels.FirstViewModel>)>]
[<Export("initWithCoder:")>]
type FirstViewController(coder: NSCoder) =
    inherit MvxViewController(coder)

    override this.ViewDidLoad() = 

        base.ViewDidLoad ()

        let set = this.CreateBindingSet<FirstViewController, Core.ViewModels.FirstViewModel>()
        //set.Bind(textFirst).To(vm => vm.Hello)
        set.Apply()