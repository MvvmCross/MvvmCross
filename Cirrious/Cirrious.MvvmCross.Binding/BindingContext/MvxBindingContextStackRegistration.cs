// MvxBindingContextStackRegistration.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.Binding.Interfaces.BindingContext;

namespace Cirrious.MvvmCross.Binding.BindingContext
{
    public class MvxBindingContextStackRegistration<TBindingContext>
        : IDisposable
    {
        protected IMvxBindingContextStack<TBindingContext> Stack
        {
            get { return Mvx.Resolve<IMvxBindingContextStack<TBindingContext>>(); }
        }

        public MvxBindingContextStackRegistration(TBindingContext toRegister)
        {
            Stack.Push(toRegister);
        }

        ~MvxBindingContextStackRegistration()
        {
            MvxTrace.Trace(MvxTraceLevel.Error, "You should always Dispose of MvxBindingContextStackRegistration");
            Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
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