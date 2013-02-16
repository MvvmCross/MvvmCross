using Cirrious.CrossCore.Touch.Views;
using Cirrious.MvvmCross.Binding.Touch.Views;

namespace Cirrious.MvvmCross.Touch.Views
{
    public static class MvxBindingAdapterExtensions
    {
        public static void AdaptForBinding(this IViewControllerEventSource view)
        {
            var adapter = new MvxViewControllerAdapter(view);
            var binding = new MvxBindingViewControllerAdapter(view);
        }
    }
}