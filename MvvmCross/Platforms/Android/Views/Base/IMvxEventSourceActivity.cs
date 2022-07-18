// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Content;
using Android.OS;
using MvvmCross.Base;

namespace MvvmCross.Platforms.Android.Views.Base
{
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
