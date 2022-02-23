// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Plugin.Messenger
{
#nullable enable
    [Preserve(AllMembers = true)]
    public class MvxSubscriberChangeMessage : MvxMessage
    {
        public Type MessageType { get; }
        public int SubscriberCount { get; }

        public MvxSubscriberChangeMessage(object sender, Type messageType, int countSubscribers = 0)
            : base(sender)
        {
            SubscriberCount = countSubscribers;
            MessageType = messageType;
        }
    }
#nullable restore
}
