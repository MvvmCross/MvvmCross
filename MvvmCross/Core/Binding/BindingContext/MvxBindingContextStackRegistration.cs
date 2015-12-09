// MvxBindingContextStackRegistration.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.BindingContext
{
    using System;

    using MvvmCross.Platform;
    using MvvmCross.Platform.Platform;

    public class MvxBindingContextStackRegistration<TBindingContext>
        : IDisposable
    {
        protected IMvxBindingContextStack<TBindingContext> Stack => Mvx.Resolve<IMvxBindingContextStack<TBindingContext>>();

        public MvxBindingContextStackRegistration(TBindingContext toRegister)
        {
            this.Stack.Push(toRegister);
        }

        ~MvxBindingContextStackRegistration()
        {
            MvxTrace.Error("You should always Dispose of MvxBindingContextStackRegistration");
            this.Dispose(false);
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
                this.Stack.Pop();
            }
        }
    }
}