// MvxInteractionExtensionMethods.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.ViewModels
{
    using System;

    using MvvmCross.Platform;
    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.WeakSubscription;

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