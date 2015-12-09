// MvxAndroidBindingContextStack.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.BindingContext
{
    using MvvmCross.Platform;

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
            return stack.Current;
        }
    }
}