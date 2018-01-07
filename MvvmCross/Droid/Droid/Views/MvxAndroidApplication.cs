using System;
using Android.App;
using Android.Runtime;

namespace MvvmCross.Droid.Views
{
    public class MvxAndroidApplication : Application, IMvxAndroidApplication
    {
        public static MvxAndroidApplication Instance { get; private set; }

        public MvxAndroidApplication()
        {
            Instance = this;
        }

        public MvxAndroidApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Instance = this;
        }
    }
}
