// IMvxAndroidActivityLifeTimeListener.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.App;
using Android.OS;
using MvvmCross.Core.Platform;
using MvvmCross.Droid.Views;

namespace MvvmCross.Droid.Platform
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