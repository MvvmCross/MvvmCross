using System;
using Android.Content;
using Android.OS;

namespace Cirrious.MvvmCross.Droid.Views
{
    public interface IActivityEventSource : IDisposeSource
    {
        event EventHandler<TypedEventArgs<Bundle>> CreateWillBeCalled;
        event EventHandler<TypedEventArgs<Bundle>> CreateCalled;
        event EventHandler DestroyCalled;
        event EventHandler<TypedEventArgs<Intent>> NewIntentCalled;
        event EventHandler ResumeCalled;
        event EventHandler PauseCalled;
        event EventHandler StartCalled;
        event EventHandler RestartCalled;
        event EventHandler StopCalled;
        event EventHandler<TypedEventArgs<StartActivityForResultParameters>> StartActivityForResultCalled;
        event EventHandler<TypedEventArgs<ActivityResultParameters>> ActivityResultCalled;
    }
}