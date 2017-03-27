namespace $rootnamespace$.ViewModels

open MvvmCross.Core.ViewModels

type MainViewModel() = 
    inherit MvxViewModel()

    let mutable hello = "Hello MvvmCross"
    member this.Hello
        with get() = hello
        and set(value) = base.SetProperty(&hello, value) |> ignore