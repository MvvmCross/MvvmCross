// MessengerHubTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using NUnit.Framework;
using System;
using System.Linq;

namespace MvvmCross.Plugins.Messenger.Test
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

        #endregion TestClasses

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

        [Test]
        public void DisposeTokenPreventsMessagesBeingReceived()
        {
            var messenger = new MvxMessengerHub();
            Action<TestMessage> action = _ => Assert.That(false, "This event should not fire!");

            var id = messenger.Subscribe(action);
            id.Dispose();
            messenger.Publish(new TestMessage(this));
        }

        [Test]
        public void NullSenderCausesException()
        {
            Assert.Throws<ArgumentNullException>(() => {
                var message = new TestMessage(null);
            });
        }

        [Test]
        public void NullSubscribeCausesException()
        {
            var messenger = new MvxMessengerHub();
            Assert.Throws<ArgumentNullException>(() => {
                messenger.Subscribe<TestMessage>(null);
            });
        }

        [Test]
        public void UnknownUnsubscribeDoesNotCauseException()
        {
            var messenger = new MvxMessengerHub();
            messenger.Unsubscribe<TestMessage>(new MvxSubscriptionToken(Guid.NewGuid(), () => { }, new object()));
            messenger.Subscribe<TestMessage>(m =>
                {
                    // stuff
                });
            messenger.Unsubscribe<TestMessage>(new MvxSubscriptionToken(Guid.NewGuid(), () => { }, new object()));
            messenger.Unsubscribe<TestMessage>(new MvxSubscriptionToken(Guid.Empty, () => { }, new object()));
        }

        [Test]
        public void NullPublishCausesException()
        {
            var messenger = new MvxMessengerHub();
            Assert.Throws<ArgumentNullException>(() => {
                messenger.Publish<TestMessage>(null);
            });
        }

        [Test]
        public void HasSubscriptionsForIsCorrect()
        {
            var messenger = new MvxMessengerHub();
            Assert.AreEqual(false, messenger.HasSubscriptionsFor<MvxSubscriberChangeMessage>());
            Assert.AreEqual(false, messenger.HasSubscriptionsFor<TestMessage>());
            var changeToken = messenger.Subscribe<MvxSubscriberChangeMessage>(message => { });
            Assert.AreEqual(true, messenger.HasSubscriptionsFor<MvxSubscriberChangeMessage>());
            Assert.AreEqual(false, messenger.HasSubscriptionsFor<TestMessage>());
            var token = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            });
            Assert.AreEqual(true, messenger.HasSubscriptionsFor<MvxSubscriberChangeMessage>());
            Assert.AreEqual(true, messenger.HasSubscriptionsFor<TestMessage>());
            messenger.Unsubscribe<TestMessage>(token);
            Assert.AreEqual(true, messenger.HasSubscriptionsFor<MvxSubscriberChangeMessage>());
            Assert.AreEqual(false, messenger.HasSubscriptionsFor<TestMessage>());
        }

        [Test]
        public void CountSubscriptionsForIsCorrect()
        {
            var messenger = new MvxMessengerHub();
            Assert.AreEqual(false, messenger.HasSubscriptionsFor<MvxSubscriberChangeMessage>());
            Assert.AreEqual(false, messenger.HasSubscriptionsFor<TestMessage>());
            var changeToken = messenger.Subscribe<MvxSubscriberChangeMessage>(message => { });
            Assert.AreEqual(1, messenger.CountSubscriptionsFor<MvxSubscriberChangeMessage>());
            Assert.AreEqual(0, messenger.CountSubscriptionsFor<TestMessage>());
            var token = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            });
            Assert.AreEqual(1, messenger.CountSubscriptionsFor<MvxSubscriberChangeMessage>());
            Assert.AreEqual(1, messenger.CountSubscriptionsFor<TestMessage>());
            var token2 = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            });
            Assert.AreEqual(1, messenger.CountSubscriptionsFor<MvxSubscriberChangeMessage>());
            Assert.AreEqual(2, messenger.CountSubscriptionsFor<TestMessage>());
            messenger.Unsubscribe<TestMessage>(token);
            Assert.AreEqual(1, messenger.CountSubscriptionsFor<MvxSubscriberChangeMessage>());
            Assert.AreEqual(1, messenger.CountSubscriptionsFor<TestMessage>());
            messenger.Unsubscribe<TestMessage>(token2);
            Assert.AreEqual(1, messenger.CountSubscriptionsFor<MvxSubscriberChangeMessage>());
            Assert.AreEqual(0, messenger.CountSubscriptionsFor<TestMessage>());
        }

        [Test]
        public void HasSubscriptionsForTagIsCorrect()
        {
            var testTag = "TestTag";
            var notExistingTag = "NotExistingTag";
            var messenger = new MvxMessengerHub();
            Assert.AreEqual(false, messenger.HasSubscriptionsFor<MvxSubscriberChangeMessage>());
            Assert.AreEqual(false, messenger.HasSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag));
            Assert.AreEqual(false, messenger.HasSubscriptionsFor<TestMessage>());
            Assert.AreEqual(false, messenger.HasSubscriptionsForTag<TestMessage>(null));
            Assert.AreEqual(false, messenger.HasSubscriptionsForTag<TestMessage>(notExistingTag));
            var changeToken = messenger.Subscribe<MvxSubscriberChangeMessage>(message => { });
            Assert.AreEqual(true, messenger.HasSubscriptionsForTag<MvxSubscriberChangeMessage>(null));
            Assert.AreEqual(false, messenger.HasSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag));
            Assert.AreEqual(false, messenger.HasSubscriptionsForTag<TestMessage>(testTag));
            Assert.AreEqual(false, messenger.HasSubscriptionsForTag<TestMessage>(null));
            Assert.AreEqual(false, messenger.HasSubscriptionsForTag<TestMessage>(notExistingTag));
            var token = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            }, tag: testTag);
            Assert.AreEqual(true, messenger.HasSubscriptionsForTag<MvxSubscriberChangeMessage>(null));
            Assert.AreEqual(false, messenger.HasSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag));
            Assert.AreEqual(true, messenger.HasSubscriptionsForTag<TestMessage>(testTag));
            Assert.AreEqual(false, messenger.HasSubscriptionsForTag<TestMessage>(null));
            Assert.AreEqual(false, messenger.HasSubscriptionsForTag<TestMessage>(notExistingTag));
            messenger.Unsubscribe<TestMessage>(token);
            Assert.AreEqual(true, messenger.HasSubscriptionsForTag<MvxSubscriberChangeMessage>(null));
            Assert.AreEqual(false, messenger.HasSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag));
            Assert.AreEqual(false, messenger.HasSubscriptionsForTag<TestMessage>(testTag));
            Assert.AreEqual(false, messenger.HasSubscriptionsForTag<TestMessage>(null));
            Assert.AreEqual(false, messenger.HasSubscriptionsForTag<TestMessage>(notExistingTag));
        }

        [Test]
        public void CountSubscriptionsForTagIsCorrect()
        {
            var testTag1 = "TestTag1";
            var testTag2 = "TestTag2";
            var notExistingTag = "NotExistingTag";
            var messenger = new MvxMessengerHub();
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(null));
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag1));
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<TestMessage>(testTag1));
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<TestMessage>(testTag2));
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<TestMessage>(notExistingTag));
            var changeToken = messenger.Subscribe<MvxSubscriberChangeMessage>(message => { });
            Assert.AreEqual(1, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(null));
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag1));
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<TestMessage>(testTag1));
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<TestMessage>(testTag2));
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<TestMessage>(notExistingTag));
            var token = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            }, tag: testTag1);
            Assert.AreEqual(1, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(null));
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag1));
            Assert.AreEqual(1, messenger.CountSubscriptionsForTag<TestMessage>(testTag1));
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<TestMessage>(testTag2));
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<TestMessage>(notExistingTag));
            var token2 = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            }, tag: testTag1);
            Assert.AreEqual(1, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(null));
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag1));
            Assert.AreEqual(2, messenger.CountSubscriptionsForTag<TestMessage>(testTag1));
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<TestMessage>(testTag2));
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<TestMessage>(notExistingTag));
            var token3 = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            }, tag: testTag2);
            Assert.AreEqual(1, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(null));
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag1));
            Assert.AreEqual(2, messenger.CountSubscriptionsForTag<TestMessage>(testTag1));
            Assert.AreEqual(1, messenger.CountSubscriptionsForTag<TestMessage>(testTag2));
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<TestMessage>(notExistingTag));
            messenger.Unsubscribe<TestMessage>(token);
            Assert.AreEqual(1, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(null));
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag1));
            Assert.AreEqual(1, messenger.CountSubscriptionsForTag<TestMessage>(testTag1));
            Assert.AreEqual(1, messenger.CountSubscriptionsForTag<TestMessage>(testTag2));
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<TestMessage>(notExistingTag));
            messenger.Unsubscribe<TestMessage>(token2);
            Assert.AreEqual(1, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(null));
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag1));
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<TestMessage>(testTag1));
            Assert.AreEqual(1, messenger.CountSubscriptionsForTag<TestMessage>(testTag2));
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<TestMessage>(notExistingTag));
            messenger.Unsubscribe<TestMessage>(token3);
            Assert.AreEqual(1, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(null));
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag1));
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<TestMessage>(testTag1));
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<TestMessage>(testTag2));
            Assert.AreEqual(0, messenger.CountSubscriptionsForTag<TestMessage>(notExistingTag));
        }

        [Test]
        public void GetSubscriptionTagsIsCorrect()
        {
            var testTag1 = "TestTag1";
            var testTag2 = "TestTag2";
            var messenger = new MvxMessengerHub();
            Assert.IsEmpty(messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>());
            Assert.IsEmpty(messenger.GetSubscriptionTagsFor<TestMessage>());
            var changeToken = messenger.Subscribe<MvxSubscriberChangeMessage>(message => { });
            Assert.AreEqual(1, messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>().Count);
            Assert.AreEqual(null, messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()[0]);
            Assert.IsEmpty(messenger.GetSubscriptionTagsFor<TestMessage>());
            var token = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            }, tag: testTag1);
            Assert.AreEqual(1, messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>().Count);
            Assert.AreEqual(null, messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()[0]);
            Assert.AreEqual(1, messenger.GetSubscriptionTagsFor<TestMessage>().Count);
            Assert.AreEqual(testTag1, messenger.GetSubscriptionTagsFor<TestMessage>()[0]);
            var token2 = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            }, tag: testTag1);
            Assert.AreEqual(1, messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>().Count);
            Assert.AreEqual(null, messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()[0]);
            Assert.AreEqual(2, messenger.GetSubscriptionTagsFor<TestMessage>().Count);
            Assert.AreEqual(testTag1, messenger.GetSubscriptionTagsFor<TestMessage>()[0]);
            Assert.AreEqual(testTag1, messenger.GetSubscriptionTagsFor<TestMessage>()[1]);
            var token3 = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            }, tag: testTag2);
            Assert.AreEqual(1, messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>().Count);
            Assert.AreEqual(null, messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()[0]);
            Assert.AreEqual(3, messenger.GetSubscriptionTagsFor<TestMessage>().Count);
            Assert.AreEqual(2, messenger.GetSubscriptionTagsFor<TestMessage>().Where(x => x == testTag1).Count());
            Assert.AreEqual(1, messenger.GetSubscriptionTagsFor<TestMessage>().Where(x => x == testTag2).Count());
            messenger.Unsubscribe<TestMessage>(token);
            Assert.AreEqual(1, messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>().Count);
            Assert.AreEqual(null, messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()[0]);
            Assert.AreEqual(2, messenger.GetSubscriptionTagsFor<TestMessage>().Count);
            Assert.AreEqual(1, messenger.GetSubscriptionTagsFor<TestMessage>().Where(x => x == testTag1).Count());
            Assert.AreEqual(1, messenger.GetSubscriptionTagsFor<TestMessage>().Where(x => x == testTag2).Count());
            messenger.Unsubscribe<TestMessage>(token2);
            Assert.AreEqual(1, messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>().Count);
            Assert.AreEqual(null, messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()[0]);
            Assert.AreEqual(1, messenger.GetSubscriptionTagsFor<TestMessage>().Count);
            Assert.AreEqual(0, messenger.GetSubscriptionTagsFor<TestMessage>().Where(x => x == testTag1).Count());
            Assert.AreEqual(1, messenger.GetSubscriptionTagsFor<TestMessage>().Where(x => x == testTag2).Count());
            messenger.Unsubscribe<TestMessage>(token3);
            Assert.AreEqual(1, messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>().Count);
            Assert.AreEqual(null, messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()[0]);
            Assert.AreEqual(0, messenger.GetSubscriptionTagsFor<TestMessage>().Count);
        }

        [Test]
        public void SubscribeAndUnsubscribeCauseChangeMessages()
        {
            var messenger = new MvxMessengerHub();
            MvxSubscriberChangeMessage subscriberChangeMessage = null;
            var changeToken = messenger.Subscribe<MvxSubscriberChangeMessage>(message => subscriberChangeMessage = message);
            var token = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            });
            Assert.NotNull(subscriberChangeMessage);
            Assert.AreEqual(1, subscriberChangeMessage.SubscriberCount);
            Assert.AreEqual(typeof(TestMessage), subscriberChangeMessage.MessageType);
            subscriberChangeMessage = null;
            messenger.Unsubscribe<TestMessage>(token);
            Assert.NotNull(subscriberChangeMessage);
            Assert.AreEqual(0, subscriberChangeMessage.SubscriberCount);
            Assert.AreEqual(typeof(TestMessage), subscriberChangeMessage.MessageType);
        }

        [Test]
        public void PurgeCausesChangeMessage()
        {
            var messenger = new MvxMessengerHub();
            MvxSubscriberChangeMessage subscriberChangeMessage = null;
            var changeToken = messenger.Subscribe<MvxSubscriberChangeMessage>(message => subscriberChangeMessage = message);
            CreateShortLivedSubscription(messenger);
            Assert.NotNull(subscriberChangeMessage);
            Assert.AreEqual(1, subscriberChangeMessage.SubscriberCount);
            Assert.AreEqual(typeof(TestMessage), subscriberChangeMessage.MessageType);
            subscriberChangeMessage = null;
            System.Threading.Thread.Sleep(100);
            GC.Collect();
            GC.WaitForFullGCComplete();
            messenger.Publish(new TestMessage(this));
            System.Threading.Thread.Sleep(100);
            Assert.NotNull(subscriberChangeMessage);
            Assert.AreEqual(0, subscriberChangeMessage.SubscriberCount);
            Assert.AreEqual(typeof(TestMessage), subscriberChangeMessage.MessageType);
        }

        private void CreateShortLivedSubscription(MvxMessengerHub messenger)
        {
            // put a large byte array in place - this encourages the garbage collector to collect
            var b = new byte[100000];
            var action = new Action<TestMessage>((message) => { b[0] = 0; });
            messenger.Subscribe<TestMessage>(action, MvxReference.Weak);
            action = null;
        }

#warning TODO - weak references need more testing here really
#warning TODO - async and ui threading need testing here really
    }
}