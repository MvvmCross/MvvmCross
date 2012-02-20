using Android.Views;

namespace Cirrious.MvvmCross.Binding.Android.Interfaces.Views
{
    public interface IMvxBindingActivity
    {
        View BindingInflate(object source, int resourceId, ViewGroup viewGroup);
        View NonBindingInflate(int resourceId, ViewGroup viewGroup);
    }
}