// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;
using MvvmCross.Base;
using MvvmCross.WeakSubscription;

namespace MvvmCross.ViewModels
{
    public static class MvxInteractionExtensions
    {
        public static IDisposable? WeakSubscribe<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] T>(
            this T interaction, EventHandler<EventArgs> action)
                where T : IMvxInteraction
        {
            var eventInfo = interaction.GetType().GetEvent("Requested");
            return eventInfo?.WeakSubscribe(interaction, action);
        }

        public static MvxValueEventSubscription<TValue>? WeakSubscribe<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] TInteraction, TValue>(
            this TInteraction interaction,
            EventHandler<MvxValueEventArgs<TValue>> action)
                where TInteraction : IMvxInteraction<TValue>
        {
            var eventInfo = interaction.GetType().GetEvent("Requested");
            return eventInfo?.WeakSubscribe(interaction, action);
        }

        public static MvxValueEventSubscription<TValue>? WeakSubscribe<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] TInteraction, TValue>(
            this TInteraction interaction, Action<TValue> action)
                where TInteraction : IMvxInteraction<TValue>
        {
            EventHandler<MvxValueEventArgs<TValue>> wrappedAction = (sender, args) => action(args.Value);
            return interaction.WeakSubscribe(wrappedAction);
        }
    }
}
