using System;
using Android.Content;
using Android.OS;
using Cirrious.CrossCore.Droid.Views;
using Cirrious.CrossCore.Interfaces.Core;

namespace Cirrious.CrossCore.Droid.Interfaces
{
    public interface IMvxActivityEventSource : IMvxDisposeSource
    {
        event EventHandler<MvxTypedEventArgs<Bundle>> CreateWillBeCalled;
        event EventHandler<MvxTypedEventArgs<Bundle>> CreateCalled;
        event EventHandler DestroyCalled;
        event EventHandler<MvxTypedEventArgs<Intent>> NewIntentCalled;
        event EventHandler ResumeCalled;
        event EventHandler PauseCalled;
        event EventHandler StartCalled;
        event EventHandler RestartCalled;
        event EventHandler StopCalled;
        event EventHandler<MvxTypedEventArgs<MvxStartActivityForResultParameters>> StartActivityForResultCalled;
        event EventHandler<MvxTypedEventArgs<MvxActivityResultParameters>> ActivityResultCalled;
    }
}