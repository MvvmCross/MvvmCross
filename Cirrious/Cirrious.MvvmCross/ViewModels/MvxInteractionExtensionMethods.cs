﻿// MvxInteractionExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.WeakSubscription;

namespace Cirrious.MvvmCross.ViewModels
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
    }
}