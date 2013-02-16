using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Interfaces;

namespace Cirrious.MvvmCross.Droid.Views
{
    public static class MvxActivityViewExtensions
    {
        public static void AddEventListeners(this IActivityEventSource activity)
        {
            if (activity is IMvxAndroidView)
            {
                var adapter = new MvxActivityAdapter(activity);
            }
            if (activity is IMvxBindingActivity)
            {
                var bindingAdapter = new MvxBindingActivityAdapter(activity);
            }
            if (activity is IMvxChildViewModelOwner)
            {
                var childOwnerAdapter = new MvxChildViewModelOwnerAdapter(activity);
            }
        }
    }
}