#region Copyright
// <copyright file="MvxAndroidTask.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Android.App;
using Android.Content;
using Cirrious.MvvmCross.Android.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Android.Platform.Tasks
{
    public class MvxAndroidTask
        : IMvxServiceConsumer<IMvxViewDispatcherProvider>
        , IMvxServiceConsumer<IMvxAndroidCurrentTopActivity>
        , IMvxServiceConsumer<IMvxIntentResultSource>
    {
        public MvxAndroidTask()
        {
        }

        private IMvxViewDispatcher ViewDispatcher
        {
            get { return this.GetService<IMvxViewDispatcherProvider>().Dispatcher; }
        }

        protected void StartActivity(Intent intent)
        {
            DoOnActivity(activity => activity.StartActivity(intent));
        }

        protected void StartActivityForResult(int requestCode, Intent intent)
        {
            DoOnActivity(activity =>
                             {
                                 var androidView = activity as IMvxAndroidView;
                                 if (androidView == null)
                                 {
                                     MvxTrace.Trace("Error - current activity is null or does not support IMvxAndroidView");
                                     return;
                                 }

                                 this.GetService<IMvxIntentResultSource>().Result += OnMvxIntentResultReceived;
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
            this.GetService<IMvxIntentResultSource>().Result -= OnMvxIntentResultReceived;
            ProcessMvxIntentResult(e);
        }

        private void DoOnMainThread(Action action)
        {
            ViewDispatcher.RequestMainThreadAction(action);
        }

        private void DoOnActivity(Action<Activity> action, bool ensureOnMainThread = true)
        {
            var activity = this.GetService<IMvxAndroidCurrentTopActivity>().Activity;

            if (ensureOnMainThread)
            {
                DoOnMainThread(() => action(activity));
            }
            else
            {
                action(activity);
            }
        }
    }
}