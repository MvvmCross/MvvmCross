using System;
using Android.App;

namespace MvvmCross.Droid.Views
{
    public class MvxActivityEventArgs : EventArgs
    {
        public MvxActivityEventArgs(Activity activity, MvxActivityState state, object extras = null)
        {
            Activity = activity;
            ActivityState = state;
            Extras = extras;
        }

        public MvxActivityState ActivityState { get; private set; }
        public Activity Activity { get; private set; }
        public object Extras { get; private set; }
    }
}