// MvxAndroidTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Droid.Platform
{
    using System;

    using Android.App;
    using Android.Content;

    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.Droid.Views;
    using MvvmCross.Platform.Platform;

    public class MvxAndroidTask
        : MvxMainThreadDispatchingObject
    {
        protected void StartActivity(Intent intent)
        {
            this.DoOnActivity(activity => activity.StartActivity(intent));
        }

        protected void StartActivityForResult(int requestCode, Intent intent)
        {
            this.DoOnActivity(activity =>
                {
                    var androidView = activity as IMvxStartActivityForResult;
                    if (androidView == null)
                    {
                        MvxTrace.Error("Error - current activity is null or does not support IMvxAndroidView");
                        return;
                    }

                    Mvx.Resolve<IMvxIntentResultSource>().Result += this.OnMvxIntentResultReceived;
                    androidView.MvxInternalStartActivityForResult(intent, requestCode);
                });
        }

        protected virtual void ProcessMvxIntentResult(MvxIntentResultEventArgs result)
        {
            // default processing does nothing
        }

        private void OnMvxIntentResultReceived(object sender, MvxIntentResultEventArgs e)
        {
            MvxTrace.Trace("OnMvxIntentResultReceived in MvxAndroidTask");
            // TODO - is this correct - should we always remove the result registration even if this isn't necessarily our result?
            Mvx.Resolve<IMvxIntentResultSource>().Result -= this.OnMvxIntentResultReceived;
            this.ProcessMvxIntentResult(e);
        }

        protected void DoOnActivity(Action<Activity> action, bool ensureOnMainThread = true)
        {
            var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

            if (ensureOnMainThread)
            {
                this.InvokeOnMainThread(() => action(activity));
            }
            else
            {
                action(activity);
            }
        }
    }
}