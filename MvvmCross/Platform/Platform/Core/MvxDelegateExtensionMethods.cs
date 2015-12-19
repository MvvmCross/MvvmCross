// MvxDelegateExtensionMethods.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Core
{
    using System;

    public static class MvxDelegateExtensionMethods
    {
        public static void Raise(this EventHandler eventHandler, object sender)
        {
            eventHandler?.Invoke(sender, EventArgs.Empty);
        }

        public static void Raise<T>(this EventHandler<MvxValueEventArgs<T>> eventHandler, object sender, T value)
        {
            eventHandler?.Invoke(sender, new MvxValueEventArgs<T>(value));
        }
    }
}