// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Content;
using Android.OS;
using MvvmCross.Base;

namespace MvvmCross.Platforms.Android.Views
{
    public interface IMvxEventSourceFragment : IMvxDisposeSource
    {
        //Created sate
        event EventHandler<MvxValueEventArgs<Context>> AttachCalled;

        event EventHandler<MvxValueEventArgs<Bundle>> CreateWillBeCalled;

        event EventHandler<MvxValueEventArgs<Bundle>> CreateCalled;

        event EventHandler<MvxValueEventArgs<MvxCreateViewParameters>> CreateViewCalled;

        //Started state
        event EventHandler StartCalled;

        //Resumed state
        event EventHandler ResumeCalled;

        //Paused state
        event EventHandler PauseCalled;

        //Stopped state
        event EventHandler StopCalled;

        //Destroyed state
        event EventHandler DestroyViewCalled;

        event EventHandler DestroyCalled;

        event EventHandler DetachCalled;

        event EventHandler<MvxValueEventArgs<Bundle>> SaveInstanceStateCalled;
    }
}
