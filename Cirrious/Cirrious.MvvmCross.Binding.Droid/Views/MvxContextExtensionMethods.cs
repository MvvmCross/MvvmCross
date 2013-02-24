// MvxContextExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public interface IMvxBindingContextStack
    {
        IMvxBindingContext Current { get; }

        void Push(IMvxBindingContext context);
        IMvxBindingContext Pop();
    }

    public class MvxDroidBindingContextStack
        : Stack<IMvxBindingContext>
          , IMvxBindingContextStack
    {
        public IMvxBindingContext Current
        {
            get { return Peek(); }
        }
    }

    public class MvxBindingContextStackRegistration
        : IMvxConsumer
          , IDisposable
    {
        protected IMvxBindingContextStack Stack
        {
            get { return this.GetService<IMvxBindingContextStack>(); }
        }

        public MvxBindingContextStackRegistration(IMvxBindingContext toRegister)
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

    /*
    public static class MvxContextExtensionMethods
    {
        public static IMvxDroidBindingContext CurrentBindingOwner(this Context context)
        {
            if (context == null)
                throw new ArgumentNullException("context", "context is null in CurrentBindingOwner");

            var bindingOwner = context as IMvxDroidBindingContext;
            if (bindingOwner == null)
                throw new ArgumentException("context must be an IMvxDroidBindingContext in CurrentBindingOwner");

            var toReturn = bindingOwner.CurrentBindingOwner();
            return (IMvxDroidBindingContext) toReturn;
        }

        public static IMvxBindingContext CurrentBindingOwner(this IMvxBindingContext bindingContext)
        {
            if (bindingContext.BindingContextHelper == null)
                throw new MvxException("IMvxDroidBindingContext cannot have a null BindingContextHelper in CurrentBindingOwner");

            var candidate = bindingContext;
            if (candidate.BindingContextHelper.CurrentOverride != null)
                return candidate.BindingContextHelper.CurrentOverride.CurrentBindingOwner();

            return candidate;
        }
    }
     **/
}