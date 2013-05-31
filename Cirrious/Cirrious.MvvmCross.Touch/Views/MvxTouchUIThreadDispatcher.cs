// MvxTouchUIThreadDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using System.Threading;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.Views
{
    public abstract class MvxTouchUIThreadDispatcher
        : MvxMainThreadDispatcher
    {
        public bool RequestMainThreadAction(Action action)
        {
            UIApplication.SharedApplication.BeginInvokeOnMainThread(() => ExceptionMaskedAction(action));
            return true;
        }
    }
}