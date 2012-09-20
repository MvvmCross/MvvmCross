#region Copyright
// <copyright file="MvxThreadSleep.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cirrious.MvvmCross.Plugins.ThreadUtils.WinRT
{
    public class MvxThreadSleep : IMvxThreadSleep
    {
        #region Implementation of IMvxThreadSleep

        public void Sleep(TimeSpan t)
        {
            Task.Delay(t).RunSynchronously();
        }

        #endregion
    }
}