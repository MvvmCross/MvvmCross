namespace $rootnamespace$.Views

open MvvmCross.Binding.BindingContext
open MvvmCross.iOS.Views

type FirstView()
    inherit MvxViewController(("FirstView", null)

    override this.ViewDidLoad() =

        base.ViewDidLoad()

        var set = this.CreateBindingSet<FirstView, Core.ViewModels.FirstViewModel>()
        set.Bind(Label).To(vm => vm.Hello)
        set.Bind(TextField).To(vm => vm.Hello)
        set.Apply()