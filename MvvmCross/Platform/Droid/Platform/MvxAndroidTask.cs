// MvxAndroidTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
using Android.Content;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Droid.Views;
using Cirrious.CrossCore.Platform;
using System;

namespace Cirrious.CrossCore.Droid.Platform
{
    public class MvxAndroidTask
        : MvxMainThreadDispatchingObject
    {
        protected void StartActivity(Intent intent)
        {
            DoOnActivity(activity => activity.StartActivity(intent));
        }

        protected void StartActivityForResult(int requestCode, Intent intent)
        {
            DoOnActivity(activity =>
                {
                    var androidView = activity as IMvxStartActivityForResult;
                    if (androidView == null)
                    {
                        MvxTrace.Error("Error - current activity is null or does not support IMvxAndroidView");
                        return;
                    }

                    Mvx.Resolve<IMvxIntentResultSource>().Result += OnMvxIntentResultReceived;
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
            Mvx.Resolve<IMvxIntentResultSource>().Result -= OnMvxIntentResultReceived;
            ProcessMvxIntentResult(e);
        }

        protected void DoOnActivity(Action<Activity> action, bool ensureOnMainThread = true)
        {
            var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

            if (ensureOnMainThread)
            {
                InvokeOnMainThread(() => action(activity));
            }
            else
            {
                action(activity);
            }
        }
    }
}