namespace $rootnamespace$.Views

open MvvmCross.Binding.BindingContext
open MvvmCross.iOS.Views

type MainView()
    inherit MvxViewController(("MainView", null)

    override this.ViewDidLoad() =

        base.ViewDidLoad()

        var set = this.CreateBindingSet<MainView, Core.ViewModels.MainViewModel>()
        set.Bind(Label).To(vm => vm.Hello)
        set.Bind(TextField).To(vm => vm.Hello)
        set.Apply()