#region Copyright
// <copyright file="IMvxAndroidActivityLifeTimeListener.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Android.App;

namespace Cirrious.MvvmCross.Android.Interfaces
{
    public interface IMvxAndroidActivityLifetimeListener
    {
        void OnCreate(Activity activity);
        void OnStart(Activity activity);
        void OnRestart(Activity activity);
        void OnResume(Activity activity);
        void OnPause(Activity activity);
        void OnStop(Activity activity);
        void OnDestroy(Activity activity);
    }
}