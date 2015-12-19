// MvxAndroidLifeTimeMonitor.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Droid.Views
{
    using Android.App;

    using MvvmCross.Core.Platform;
    using MvvmCross.Droid.Platform;
    using MvvmCross.Platform.Droid.Platform;

    // For lifetime explained, see http://developer.android.com/guide/topics/fundamentals/activities.html
    // Note that we set Activity = activity in multiple places
    // basically we just want to intercept the activity as early as possible
    // regardless of whether the activity has come from an app switch or a new start or...
    public class MvxAndroidLifetimeMonitor
        : MvxLifetimeMonitor
          , IMvxAndroidActivityLifetimeListener
          , IMvxAndroidCurrentTopActivity
    {
        private int _createdActivityCount;

        #region IMvxAndroidActivityLifetimeListener Members

        public virtual void OnCreate(Activity activity)
        {
            this._createdActivityCount++;
            if (this._createdActivityCount == 1)
            {
                FireLifetimeChange(MvxLifetimeEvent.ActivatedFromDisk);
            }
            this.Activity = activity;
        }

        public virtual void OnStart(Activity activity)
        {
            this.Activity = activity;
        }

        public virtual void OnRestart(Activity activity)
        {
            this.Activity = activity;
        }

        public virtual void OnResume(Activity activity)
        {
            this.Activity = activity;
        }

        public virtual void OnPause(Activity activity)
        {
            // ignored
        }

        public virtual void OnStop(Activity activity)
        {
            // ignored
        }

        public virtual void OnDestroy(Activity activity)
        {
            if (this.Activity == activity)
                this.Activity = null;

            this._createdActivityCount--;
            if (this._createdActivityCount == 0)
            {
                FireLifetimeChange(MvxLifetimeEvent.Closing);
            }
        }

        public virtual void OnViewNewIntent(Activity activity)
        {
            this.Activity = activity;
        }

        #endregion IMvxAndroidActivityLifetimeListener Members

        #region IMvxAndroidCurrentTopActivity Members

        public Activity Activity { get; private set; }

        #endregion IMvxAndroidCurrentTopActivity Members
    }
}