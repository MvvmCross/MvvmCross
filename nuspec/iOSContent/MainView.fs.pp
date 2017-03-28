namespace $rootnamespace$.Views

open MvvmCross.Binding.BindingContext
open MvvmCross.iOS.Views

type MainView() =
    inherit MvxViewController("MainView", null)

    override this.ViewDidLoad() =

        base.ViewDidLoad()

        let set = this.CreateBindingSet<MainView, Core.ViewModels.MainViewModel>()
        set.Bind("Label").To("Hello") |> ignore
        set.Bind("TextField").To("ResetTextCommand") |> ignore
        set.Apply()