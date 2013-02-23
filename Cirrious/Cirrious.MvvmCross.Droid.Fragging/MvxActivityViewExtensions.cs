namespace Cirrious.MvvmCross.Droid.Fragging
{
    public static class MvxActivityViewExtensions
    {
        public static void AddEventListeners(this IMvxEventSourceFragment fragment)
        {
            if (fragment is IMvxAndroidFragmentView)
            {
                var adapter = new MvxBindingFragmentAdapter(fragment);
            }
        }
    }
}