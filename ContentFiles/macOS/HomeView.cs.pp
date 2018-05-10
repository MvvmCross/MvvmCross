using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Mac.Views;
using System;

namespace $rootnamespace$.Views
{
    [MvxFromStoryboard("Home")]
    public partial class HomeView : MvxViewController<HomeViewModel>
    {
        public HomeView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<HomeView, HomeViewModel>();
            set.Bind(TextField).To(vm => vm.Text);
            set.Bind(Button).To(vm => vm.ResetTextCommand);
            set.Apply();
        }
    }
}
