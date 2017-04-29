// MvxBindingContextStackRegistration.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Binding.BindingContext
{
    public class MvxBindingContextStackRegistration<TBindingContext>
        : IDisposable
    {
        public MvxBindingContextStackRegistration(TBindingContext toRegister)
        {
            Stack.Push(toRegister);
        }

        protected IMvxBindingContextStack<TBindingContext> Stack => Mvx
            .Resolve<IMvxBindingContextStack<TBindingContext>>();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MvxBindingContextStackRegistration()
        {
            MvxTrace.Error("You should always Dispose of MvxBindingContextStackRegistration");
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                Stack.Pop();
        }
    }
}