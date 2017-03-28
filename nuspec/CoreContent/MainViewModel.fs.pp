namespace $rootnamespace$.ViewModels

open System
open MvvmCross.Core.ViewModels

type MainViewModel() as this = 
    inherit MvxViewModel()

    let mutable text = "Hello MvvmCross"

    // Even though there's no Init method on MvxViewModel, this method is used by
    // the framework to pass information when navigating from one VM to another.
    // Every ViewModel can have its own Init method, as long as they either:
    // 1 - Implement MvxViewModel<TInit> 
    // 2 - Follow the guidelines from this document: https://github.com/MvvmCross/MvvmCross/wiki/view-model-lifecycle
    // If you choose the former, avoid serializing large objects
    // If you choose the latter, avoid doing asynchronous operations on Init, prefer doing them on the Start method.
    // member this.Init(id: int) =
    //     this.id <- id

    /// The start method is called as the last step of the ViewModel initialization.
    /// Even though it is a void method, if you need to do any async work it should be done here.
    override this.Start() =
        //TODO: Add starting logic here
        ()

    member this.Text
        with get() = text
        and set(value) = base.SetProperty(&text, value) |> ignore

    member this.ResetTextCommandExecute() =
        this.Text <- "Hello MvvmCross"
        ()

    /// Commands are a way to bind events that happen on your View to methods from the ViewModel 
    member val ResetTextCommand = MvxCommand(Action(this.ResetTextCommandExecute))