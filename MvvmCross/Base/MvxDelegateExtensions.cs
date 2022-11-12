// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Base
{
#nullable enable
    public static class MvxDelegateExtensions
    {
#pragma warning disable CA1030 // Use events where appropriate
        public static void Raise(this EventHandler eventHandler, object sender)

        {
            eventHandler?.Invoke(sender, EventArgs.Empty);
        }

        public static void Raise<T>(this EventHandler<MvxValueEventArgs<T>> eventHandler, object sender, T value)
        {
            eventHandler?.Invoke(sender, new MvxValueEventArgs<T>(value));
        }
#pragma warning restore CA1030 // Use events where appropriate
    }
#nullable restore
}
