using Android.OS;
using Android.Views;

namespace Cirrious.MvvmCross.Droid.Fragging
{
    public class MvxCreateViewParameters
    {
        public MvxCreateViewParameters(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            SavedInstanceState = savedInstanceState;
            Container = container;
            Inflater = inflater;
        }

        public LayoutInflater Inflater { get; private set; }
        public ViewGroup Container { get; private set; }
        public Bundle SavedInstanceState { get; private set; }
    }
}