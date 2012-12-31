#region Copyright

// <copyright file="MessengerHubTest.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using NUnit.Framework;

namespace Cirrious.MvvmCross.Plugins.Messenger.Test
{
    [TestFixture]
    public class MessengerHubTest
    {
        #region TestClasses

        private class TestMessage : BaseMessage
        {
            public TestMessage(object sender)
                : base(sender)
            {
            }
        }

        private class OtherTestMessage : BaseMessage
        {
            public OtherTestMessage(object sender)
                : base(sender)
            {
            }
        }

        #endregion

        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void SubscribeAndPublishAllowsMessageToBeReceived()
        {
            var messenger = new MessengerHub();
            var message = new TestMessage(this);

            var messageReceived = false;
            messenger.Subscribe<TestMessage>(m =>
                {
                    Assert.That(m, Is.EqualTo(message));
                    Assert.That(m.Sender, Is.EqualTo(this));
                    messageReceived = true;
                });

            messenger.Publish(message);

            Assert.IsTrue(messageReceived);
        }

        [Test]
        public void MultipleSubscribeAndPublishAllowsMessageToBeReceived()
        {
            var messenger = new MessengerHub();
            var message = new TestMessage(this);
            var otherMessage = new OtherTestMessage(this);

            var messageReceived = 0;
            messenger.Subscribe<TestMessage>(m =>
                {
                    Assert.That(m, Is.EqualTo(message));
                    Assert.That(m.Sender, Is.EqualTo(this));
                    messageReceived++;
                });

            var otherMessageReceived = 0;
            messenger.Subscribe<OtherTestMessage>(m =>
                {
                    Assert.That(m, Is.EqualTo(otherMessage));
                    Assert.That(m.Sender, Is.EqualTo(this));
                    otherMessageReceived++;
                });

            messenger.Publish(otherMessage);
            Assert.AreEqual(0, messageReceived);
            Assert.AreEqual(1, otherMessageReceived);

            messenger.Publish(message);
            Assert.AreEqual(1, messageReceived);
            Assert.AreEqual(1, otherMessageReceived);

            messenger.Publish(message);
            Assert.AreEqual(2, messageReceived);
            Assert.AreEqual(1, otherMessageReceived);

            messenger.Publish(message);
            Assert.AreEqual(3, messageReceived);
            Assert.AreEqual(1, otherMessageReceived);

            messenger.Publish(otherMessage);
            Assert.AreEqual(3, messageReceived);
            Assert.AreEqual(2, otherMessageReceived);
        }

        [Test]
        public void UnsubscribePreventsMessagesBeingReceived()
        {
            var messenger = new MessengerHub();
            Action<TestMessage> action = _ => Assert.That(false, "This event should not fire!");

            var id = messenger.Subscribe(action);
            messenger.Unsubscribe<TestMessage>(id);
            messenger.Publish(new TestMessage(this));
        }

        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void NullSenderCausesException()
        {
            var message = new TestMessage(null);
        }

        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void NullSubscribeCausesException()
        {
            var messenger = new MessengerHub();
            messenger.Subscribe<TestMessage>(null);
        }


        [Test]
        public void UnknownUnsubscribeDoesNotCauseException()
        {
            var messenger = new MessengerHub();
            messenger.Unsubscribe<TestMessage>(Guid.NewGuid());
            messenger.Subscribe<TestMessage>(m =>
                {
                    // stuff
                });
            messenger.Unsubscribe<TestMessage>(Guid.NewGuid());
            messenger.Unsubscribe<TestMessage>(Guid.Empty);
        }

        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void NullPublishCausesException()
        {
            var messenger = new MessengerHub();
            messenger.Publish<TestMessage>(null);
        }
    }
}