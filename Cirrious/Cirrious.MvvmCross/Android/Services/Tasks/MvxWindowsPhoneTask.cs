#region Copyright

// <copyright file="MvxWindowsPhoneTask.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using Android.App;
using Android.Content;
using Cirrious.MvvmCross.Android.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.Android.Services.Tasks
{
    public class MvxWindowsPhoneTask
        : IMvxServiceConsumer<IMvxViewDispatcherProvider>
          , IMvxServiceConsumer<IMvxAndroidActivityTracker>
    {
        private IMvxViewDispatcher ViewDispatcher
        {
            get { return this.GetService<IMvxViewDispatcherProvider>().Dispatcher; }
        }

        private Activity Activity
        {
            get { return this.GetService<IMvxAndroidActivityTracker>().CurrentTopLevelActivity; }
        }

        protected void StartActivity(Intent intent)
        {
            DoOnActivity(activity => activity.StartActivity(intent));
        }

        private void DoOnMainThread(Action action)
        {
            ViewDispatcher.RequestMainThreadAction(action);
        }

        private void DoOnActivity(Action<Activity> action)
        {
            DoOnActivity(action, true);
        }

        private void DoOnActivity(Action<Activity> action, bool ensureOnMainThread)
        {
            if (ensureOnMainThread)
            {
                DoOnMainThread(() => action(Activity));
            }
            else
            {
                action(Activity);
            }
        }
    }
}