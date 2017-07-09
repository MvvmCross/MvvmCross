using System;
using Android.App;

namespace MvvmCross.Droid.Views
{
    public class MvxActivityEventArgs : EventArgs
    {
        public MvxActivityEventArgs(Activity activity, MvxActivityState state)
        {
            Activity = activity;
            ActivityState = state;
        }

        public MvxActivityState ActivityState { get; private set; }
        public Activity Activity { get; private set; }
    }
}