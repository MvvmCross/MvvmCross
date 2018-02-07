// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Platform;

namespace MvvmCross.Plugin.Messenger
{
    [Preserve(AllMembers = true)]
	public class MvxSubscriberChangeMessage : MvxMessage
    {
        public Type MessageType { get; private set; }
        public int SubscriberCount { get; private set; }

        public MvxSubscriberChangeMessage(object sender, Type messageType, int countSubscribers = 0)
            : base(sender)
        {
            SubscriberCount = countSubscribers;
            MessageType = messageType;
        }
    }
}
