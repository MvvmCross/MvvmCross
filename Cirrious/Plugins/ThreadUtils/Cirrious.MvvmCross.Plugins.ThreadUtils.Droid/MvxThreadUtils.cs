#region Copyright
// <copyright file="MvxThreadUtils.cs">
// (c) Copyright Cheesebaron. http://ostebaronen.dk
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Contributor - Tomasz Cielecki. http://ostebaronen.dk
#endregion

using System;
using System.Globalization;
using System.Threading;

namespace Cirrious.MvvmCross.Plugins.ThreadUtils.Droid
{
    public class MvxThreadUtils : IMvxThreadUtils
    {
        public Thread MvxThread(Action method)
        {
            return MvxThread(method, null, false);
        }

        public Thread MvxThread(Action method, string name, bool isBackground)
        {
            return MvxThread(method, name, isBackground, null, null);
        }

        public Thread MvxThread(Action method, string name, bool isBackground, CultureInfo currentCulture, CultureInfo currentUiCulture)
        {
            if (null == method) throw new ArgumentNullException("method");
            var t = new Thread(() => InvokeMethod(method)) { IsBackground = isBackground };

            t.Name = !string.IsNullOrEmpty(name) ? name : string.Format("{0} thread {1}", method.Method.Name, t.GetHashCode());

            if (null != currentCulture)
                t.CurrentCulture = currentCulture;

            if (null != currentUiCulture)
                t.CurrentUICulture = currentUiCulture;

            return t;
        }

        private static void InvokeMethod(Action action)
        {
            action();
        }

        public void Join(Thread thread)
        {
            thread.Join();
        }

        public void Start(Thread thread)
        {
            thread.Start();
        }

        public void Start(Thread thread, object obj)
        {
            thread.Start(obj);
        }

        public void Sleep(TimeSpan span)
        {
            Thread.Sleep(span);
        }

        public void Sleep(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }
    }
}