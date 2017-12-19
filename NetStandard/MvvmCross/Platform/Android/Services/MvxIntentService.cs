using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using MvvmCross.Droid.Platform;

namespace MvvmCross.Droid.Services
{
    [Register("mvvmcross.droid.services.MvxIntentService")]
    public abstract class MvxIntentService : IntentService
    {
        protected MvxIntentService(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected MvxIntentService(string name) : base(name)
        {
        }

        protected override void OnHandleIntent(Intent intent)
        {
            var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext);
            setup.EnsureInitialized();
        }
    }
}