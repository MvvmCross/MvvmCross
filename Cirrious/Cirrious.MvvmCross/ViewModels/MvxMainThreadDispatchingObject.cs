// MvxMainThreadDispatchingObject.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.ViewModels
{
    public abstract class MvxMainThreadDispatchingObject
        : IMvxServiceConsumer
    {
        protected IMvxViewDispatcher ViewDispatcher
        {
			get { return this.GetService<IMvxViewDispatcherProvider>().Dispatcher; }
        }

        protected void InvokeOnMainThread(Action action)
        {
            if (ViewDispatcher != null)
                ViewDispatcher.RequestMainThreadAction(action);
        }
    }
}