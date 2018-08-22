// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding.BindingContext;

namespace MvvmCross.Platforms.Android.Binding.BindingContext
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
            var stack = Mvx.IoCProvider.Resolve<IMvxBindingContextStack<T>>();
            return stack.Current;
        }
    }
}
