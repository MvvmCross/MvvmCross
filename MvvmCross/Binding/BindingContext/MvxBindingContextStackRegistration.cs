// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Logging;

namespace MvvmCross.Binding.BindingContext
{
    public class MvxBindingContextStackRegistration<TBindingContext>
        : IDisposable
    {
        protected IMvxBindingContextStack<TBindingContext> Stack => Mvx.IoCProvider.Resolve<IMvxBindingContextStack<TBindingContext>>();

        public MvxBindingContextStackRegistration(TBindingContext toRegister)
        {
            Stack.Push(toRegister);
        }

        ~MvxBindingContextStackRegistration()
        {
            MvxLog.Instance.Error("You should always Dispose of MvxBindingContextStackRegistration");
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stack.Pop();
            }
        }
    }
}