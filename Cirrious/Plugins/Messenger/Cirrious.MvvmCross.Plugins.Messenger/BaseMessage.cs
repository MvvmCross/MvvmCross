﻿using System;
using System.Text;

namespace Cirrious.MvvmCross.Plugins.Messenger
{
    /// <summary>
    /// Base class for messages that provides weak refrence storage of the sender
    /// </summary>
    public abstract class BaseMessage
    {
        /// <summary>
        /// Gets the original sender of the message
        /// </summary>
        public object Sender
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the BaseMessage class.
        /// </summary>
        /// <param name="sender">Message sender (usually "this")</param>
        protected BaseMessage(object sender)
        {
            if (sender == null)
                throw new ArgumentNullException("sender");

            Sender = sender;
        }
    }
}
