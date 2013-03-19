// MessengerHubTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using NUnit.Framework;

namespace Cirrious.MvvmCross.Plugins.Messenger.Test
{
    [TestFixture]
    public class MessengerHubTest
    {
        #region TestClasses

        private class TestMessage : MvxMessage
        {
            public TestMessage(object sender)
                : base(sender)
            {
            }
        }

        private class OtherTestMessage : MvxMessage
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
            var messenger = new MvxMessengerHub();
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
            var messenger = new MvxMessengerHub();
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
            var messenger = new MvxMessengerHub();
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
            var messenger = new MvxMessengerHub();
            messenger.Subscribe<TestMessage>(null);
        }


        [Test]
        public void UnknownUnsubscribeDoesNotCauseException()
        {
            var messenger = new MvxMessengerHub();
            messenger.Unsubscribe<TestMessage>(new MvxSubscriptionToken(Guid.NewGuid(), new object()));
            messenger.Subscribe<TestMessage>(m =>
                {
                    // stuff
                });
            messenger.Unsubscribe<TestMessage>(new MvxSubscriptionToken(Guid.NewGuid(), new object()));
            messenger.Unsubscribe<TestMessage>(new MvxSubscriptionToken(Guid.Empty, new object()));
        }

        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void NullPublishCausesException()
        {
            var messenger = new MvxMessengerHub();
            messenger.Publish<TestMessage>(null);
        }

#warning TODO - weak references need testing here really
#warning TODO - async and ui threading need testing here really
    }
}