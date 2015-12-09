namespace MvvmCross.Binding.Droid.Binders
{
    using Android.Content;
    using Android.Util;
    using Android.Views;

    public interface IMvxLayoutInflaterFactory
    {
        View OnCreateView(View parent, string name, Context context, IAttributeSet attrs);
    }
}