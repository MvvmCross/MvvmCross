// MvxSubscriberChangeMessage.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Plugins.Messenger
{
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