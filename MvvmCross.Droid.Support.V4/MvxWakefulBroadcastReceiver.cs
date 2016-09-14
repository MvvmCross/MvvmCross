using System;
using Android.Content;
using Android.Runtime;
using Android.Support.V4.Content;
using MvvmCross.Droid.Platform;

namespace MvvmCross.Droid.Support.V4
{
    public abstract class MvxWakefulBroadcastReceiver : WakefulBroadcastReceiver
    {
        protected MvxWakefulBroadcastReceiver(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected MvxWakefulBroadcastReceiver()
        {
        }

        public override void OnReceive(Context context, Intent intent)
        {
            var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(context);
            setup.EnsureInitialized();
        }
    }
}