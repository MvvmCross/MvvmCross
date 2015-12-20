// IMvxAndroidActivityLifeTimeListener.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Droid.Platform
{
    using Android.App;

    public interface IMvxAndroidActivityLifetimeListener
    {
        void OnCreate(Activity activity);

        void OnStart(Activity activity);

        void OnRestart(Activity activity);

        void OnResume(Activity activity);

        void OnPause(Activity activity);

        void OnStop(Activity activity);

        void OnDestroy(Activity activity);

        void OnViewNewIntent(Activity activity);
    }
}