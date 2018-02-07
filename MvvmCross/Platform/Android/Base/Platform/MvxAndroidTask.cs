// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.App;
using Android.Content;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Droid.Views;
using MvvmCross.Platform.Logging;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Platform.Android.Base.Platform
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
                        MvxLog.Instance.Error("Error - current activity is null or does not support IMvxAndroidView");
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
            MvxLog.Instance.Trace("OnMvxIntentResultReceived in MvxAndroidTask");
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
