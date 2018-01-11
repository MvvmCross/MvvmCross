using System;
using Android.App;
using Android.Runtime;
using MvvmCross.Droid.Views;

namespace Playground.Droid
{
    [Application]
    public class MainApplication : MvxAndroidApplication
    {
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }
    }
}
