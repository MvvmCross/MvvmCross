// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Plugin.Messenger
{
    /// <summary>
    /// Base class for messages that provides weak refrence storage of the sender
    /// </summary>
    public abstract class MvxMessage
    {
        /// <summary>
        /// Gets the original sender of the message
        /// </summary>
        public object Sender { get; private set; }

        /// <summary>
        /// Initializes a new instance of the MvxMessage class.
        /// </summary>
        /// <param name="sender">Message sender (usually "this")</param>
        protected MvxMessage(object sender)
        {
            if (sender == null)
                throw new ArgumentNullException(nameof(sender));

            Sender = sender;
        }
    }
}
