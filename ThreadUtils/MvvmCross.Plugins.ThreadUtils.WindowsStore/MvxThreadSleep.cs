// MvxThreadSleep.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Platform;

namespace MvvmCross.Plugins.ThreadUtils.WindowsStore
{
    public class MvxThreadSleep : IMvxThreadSleep
    {
        #region Implementation of IMvxThreadSleep

        public void Sleep(TimeSpan t)
        {
            MvxTrace.Trace("Sleep not implemented on WinRT - asynchronous APIs must be used");
            //Task.Delay(t).RunSynchronously();
        }

        #endregion
    }
}