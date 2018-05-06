using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Tvos.Presenters.Attributes;
using MvvmCross.Platforms.Tvos.Views;

namespace $rootnamespace$.Views
{
    [MvxFromStoryboard("Home")]
    [MvxRootPresentation(WrapInNavigationController = true)]
    public partial class HomeView : MvxViewController
    {
        public HomeView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<HomeView, Core.ViewModels.HomeViewModel>();
            set.Bind(TextField).To(vm => vm.Text);
            set.Bind(Button).To(vm => vm.ResetTextCommand);
            set.Apply();
        }
    }
}
