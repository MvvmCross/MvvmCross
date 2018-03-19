// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.App;

namespace MvvmCross.Platform.Android.Views
{
    public class MvxActivityEventArgs : EventArgs
    {
        public MvxActivityEventArgs(Activity activity, MvxActivityState state, object extras = null)
        {
            Activity = activity;
            ActivityState = state;
            Extras = extras;
        }

        public MvxActivityState ActivityState { get; private set; }
        public Activity Activity { get; private set; }
        public object Extras { get; private set; }
    }
}
