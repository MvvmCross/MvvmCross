#region Copyright
// <copyright file="IMvxThreadUtils.cs">
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

namespace Cirrious.MvvmCross.Plugins.ThreadUtils
{
    public interface IMvxThreadUtils
    {
        Thread MvxThread(Action method);
        Thread MvxThread(Action method, string name, bool isBackground);
        Thread MvxThread(Action method, string name, bool isBackground, CultureInfo currentCulture, CultureInfo currentUiCulture);
        void Join(Thread thread);
        void Start(Thread thread);
        void Start(Thread thread, object obj);
        void Sleep(TimeSpan span);
        void Sleep(int milliseconds);
    }
}
