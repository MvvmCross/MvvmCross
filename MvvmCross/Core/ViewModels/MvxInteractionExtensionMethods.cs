// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Base.Core;
using MvvmCross.Base.WeakSubscription;

namespace MvvmCross.Core.ViewModels
{
    public static class MvxInteractionExtensionMethods
    {
        public static IDisposable WeakSubscribe(this IMvxInteraction interaction, EventHandler<EventArgs> action)
        {
            var eventInfo = interaction.GetType().GetEvent("Requested");
            return eventInfo.WeakSubscribe(interaction, action);
        }

        public static MvxValueEventSubscription<T> WeakSubscribe<T>(this IMvxInteraction<T> interaction, EventHandler<MvxValueEventArgs<T>> action)
        {
            var eventInfo = interaction.GetType().GetEvent("Requested");
            return eventInfo.WeakSubscribe<T>(interaction, action);
        }

        public static MvxValueEventSubscription<T> WeakSubscribe<T>(this IMvxInteraction<T> interaction, Action<T> action)
        {
            EventHandler<MvxValueEventArgs<T>> wrappedAction = (sender, args) => action(args.Value);
            return interaction.WeakSubscribe(wrappedAction);
        }
    }
}