// IMvxEventSourceActivity.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Droid.Views
{
    using System;

    using Android.Content;
    using Android.OS;

    using MvvmCross.Platform.Core;

    public interface IMvxEventSourceActivity : IMvxDisposeSource
    {
        event EventHandler<MvxValueEventArgs<Bundle>> CreateWillBeCalled;

        event EventHandler<MvxValueEventArgs<Bundle>> CreateCalled;

        event EventHandler DestroyCalled;

        event EventHandler<MvxValueEventArgs<Intent>> NewIntentCalled;

        event EventHandler ResumeCalled;

        event EventHandler PauseCalled;

        event EventHandler StartCalled;

        event EventHandler RestartCalled;

        event EventHandler StopCalled;

        event EventHandler<MvxValueEventArgs<Bundle>> SaveInstanceStateCalled;

        event EventHandler<MvxValueEventArgs<MvxStartActivityForResultParameters>> StartActivityForResultCalled;

        event EventHandler<MvxValueEventArgs<MvxActivityResultParameters>> ActivityResultCalled;
    }
}