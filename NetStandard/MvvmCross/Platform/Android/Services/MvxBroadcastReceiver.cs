using System;
using Android.Content;
using Android.Runtime;
using MvvmCross.Droid.Platform;

namespace MvvmCross.Droid.Services
{
    [Register("mvvmcross.droid.services.MvxBroadcastReceiver")]
    public abstract class MvxBroadcastReceiver : BroadcastReceiver
    {
        protected MvxBroadcastReceiver(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected MvxBroadcastReceiver()
        {
        }

        public override void OnReceive(Context context, Intent intent)
        {
            var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(context);
            setup.EnsureInitialized();
        }
    }
}