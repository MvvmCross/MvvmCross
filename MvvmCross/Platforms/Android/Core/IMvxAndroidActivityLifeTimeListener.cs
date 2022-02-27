// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.App;
using Android.OS;
using MvvmCross.Core;
using MvvmCross.Platforms.Android.Views;

namespace MvvmCross.Platforms.Android.Core
{
    public interface IMvxAndroidActivityLifetimeListener : IMvxLifetime
    {
        void OnCreate(Activity activity, Bundle eventArgs);

        void OnStart(Activity activity);

        void OnRestart(Activity activity);

        void OnResume(Activity activity);

        void OnPause(Activity activity);

        void OnStop(Activity activity);

        void OnDestroy(Activity activity);

        void OnViewNewIntent(Activity activity);

        void OnSaveInstanceState(Activity activity, Bundle eventArgs);

        event EventHandler<MvxActivityEventArgs> ActivityChanged;
    }
}
