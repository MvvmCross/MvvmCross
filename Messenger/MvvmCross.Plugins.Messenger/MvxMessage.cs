// MvxMessage.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Plugins.Messenger
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