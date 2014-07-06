// MvxAndroidBindingContextStack.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace Cirrious.MvvmCross.Binding.Droid.BindingContext
{
    public static class MvxAndroidBindingContextHelpers
    {
        public static IMvxAndroidBindingContext Current()
        {
            return Current<IMvxAndroidBindingContext>();
        }

        public static T Current<T>()
            where T : class, IMvxBindingContext
        {
            var stack = Mvx.Resolve<IMvxBindingContextStack<T>>();
            var current = stack.Current;
            if (current == null)
            {
                MvxBindingTrace.Error("You should not try to get the current bindingcontext stack entry when the stack is empty.");
            }

            return current;
        }
    }
}