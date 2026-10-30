// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading;
using MvvmCross.Plugin.Messenger;
using MvvmCross.Tests;

namespace MvvmCross.Plugin.Messenger.UnitTest
{
    [ClassDataSource<MvxTestFixture>(Shared = SharedType.PerClass)]
    public class MessengerHubTest
    {
        private readonly MvxTestFixture _fixture;

        public MessengerHubTest(MvxTestFixture fixture)
        {
            _fixture = fixture;
        }

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


        [Test]
        public async Task SubscribeAndPublishAllowsMessageToBeReceived()
        {
            var messenger = new MvxMessengerHub();
            var message = new TestMessage(this);

            var receivedMessages = new List<TestMessage>();
            using var subscription = messenger.Subscribe<TestMessage>(receivedMessages.Add);

            messenger.Publish(message);

            var received = await Assert.That(receivedMessages).HasSingleItem();
            await Assert.That(received).IsEqualTo(message);
            await Assert.That(received.Sender).IsEqualTo(this);
        }

        [Test]
        public async Task MultipleSubscribeAndPublishAllowsMessageToBeReceived()
        {
            var messenger = new MvxMessengerHub();
            var message = new TestMessage(this);
            var otherMessage = new OtherTestMessage(this);

            var messageReceived = 0;
            var receivedMessages = new List<TestMessage>();
            messenger.Subscribe<TestMessage>(m =>
            {
                receivedMessages.Add(m);
                messageReceived++;
            });

            var otherMessageReceived = 0;
            var otherReceivedMessages = new List<OtherTestMessage>();
            messenger.Subscribe<OtherTestMessage>(m =>
            {
                otherReceivedMessages.Add(m);
                otherMessageReceived++;
            });

            messenger.Publish(otherMessage);
            await Assert.That(messageReceived).IsEqualTo(0);
            await Assert.That(otherMessageReceived).IsEqualTo(1);

            messenger.Publish(message);
            await Assert.That(messageReceived).IsEqualTo(1);
            await Assert.That(otherMessageReceived).IsEqualTo(1);

            messenger.Publish(message);
            await Assert.That(messageReceived).IsEqualTo(2);
            await Assert.That(otherMessageReceived).IsEqualTo(1);

            messenger.Publish(message);
            await Assert.That(messageReceived).IsEqualTo(3);
            await Assert.That(otherMessageReceived).IsEqualTo(1);

            messenger.Publish(otherMessage);
            await Assert.That(messageReceived).IsEqualTo(3);
            await Assert.That(otherMessageReceived).IsEqualTo(2);

            foreach (var received in receivedMessages)
            {
                await Assert.That(received).IsEqualTo(message);
                await Assert.That(received.Sender).IsEqualTo(this);
            }
            foreach (var received in otherReceivedMessages)
            {
                await Assert.That(received).IsEqualTo(otherMessage);
                await Assert.That(received.Sender).IsEqualTo(this);
            }
        }

        [Test]
        public async Task UnsubscribePreventsMessagesBeingReceived()
        {
            var messenger = new MvxMessengerHub();
            var messageReceived = false;
            Action<TestMessage> action = _ => messageReceived = true;

            var id = messenger.Subscribe(action);
            messenger.Unsubscribe<TestMessage>(id);
            messenger.Publish(new TestMessage(this));
            await Assert.That(messageReceived).IsFalse();
        }

        [Test]
        public async Task DisposeTokenPreventsMessagesBeingReceived()
        {
            var messenger = new MvxMessengerHub();
            var messageReceived = false;
            Action<TestMessage> action = _ => messageReceived = true;

            var id = messenger.Subscribe(action);
            id.Dispose();
            messenger.Publish(new TestMessage(this));
            await Assert.That(messageReceived).IsFalse();
        }

        [Test]
        public void NullSenderCausesException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var message = new TestMessage(null);
            });
        }

        [Test]
        public void NullSubscribeCausesException()
        {
            var messenger = new MvxMessengerHub();
            Assert.Throws<ArgumentNullException>(() => messenger.Subscribe<TestMessage>(null));
        }

        [Test]
        public async Task UnknownUnsubscribeDoesNotCauseException()
        {
            var messenger = new MvxMessengerHub();
            messenger.Unsubscribe<TestMessage>(new MvxSubscriptionToken(Guid.NewGuid(), () => { }, new object()));
            messenger.Subscribe<TestMessage>(_ =>
            {
                // stuff
            });
            messenger.Unsubscribe<TestMessage>(new MvxSubscriptionToken(Guid.NewGuid(), () => { }, new object()));
            messenger.Unsubscribe<TestMessage>(new MvxSubscriptionToken(Guid.Empty, () => { }, new object()));

            await Assert.That(messenger.HasSubscriptionsFor<TestMessage>()).IsTrue();
        }

        [Test]
        public void NullPublishCausesException()
        {
            var messenger = new MvxMessengerHub();
            Assert.Throws<ArgumentNullException>(() =>
            {
                messenger.Publish<TestMessage>(null);
            });
        }

        [Test]
        public async Task HasSubscriptionsForIsCorrect()
        {
            var messenger = new MvxMessengerHub();
            await Assert.That(messenger.HasSubscriptionsFor<MvxSubscriberChangeMessage>()).IsFalse();
            await Assert.That(messenger.HasSubscriptionsFor<TestMessage>()).IsFalse();
            var changeToken = messenger.Subscribe<MvxSubscriberChangeMessage>(message => { });
            await Assert.That(messenger.HasSubscriptionsFor<MvxSubscriberChangeMessage>()).IsTrue();
            await Assert.That(messenger.HasSubscriptionsFor<TestMessage>()).IsFalse();
            var token = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            });
            await Assert.That(messenger.HasSubscriptionsFor<MvxSubscriberChangeMessage>()).IsTrue();
            await Assert.That(messenger.HasSubscriptionsFor<TestMessage>()).IsTrue();
            messenger.Unsubscribe<TestMessage>(token);
            await Assert.That(messenger.HasSubscriptionsFor<MvxSubscriberChangeMessage>()).IsTrue();
            await Assert.That(messenger.HasSubscriptionsFor<TestMessage>()).IsFalse();
        }

        [Test]
        public async Task CountSubscriptionsForIsCorrect()
        {
            var messenger = new MvxMessengerHub();
            await Assert.That(messenger.HasSubscriptionsFor<MvxSubscriberChangeMessage>()).IsFalse();
            await Assert.That(messenger.HasSubscriptionsFor<TestMessage>()).IsFalse();
            var changeToken = messenger.Subscribe<MvxSubscriberChangeMessage>(message => { });
            await Assert.That(messenger.CountSubscriptionsFor<MvxSubscriberChangeMessage>()).IsEqualTo(1);
            await Assert.That(messenger.CountSubscriptionsFor<TestMessage>()).IsEqualTo(0);
            var token = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            });
            await Assert.That(messenger.CountSubscriptionsFor<MvxSubscriberChangeMessage>()).IsEqualTo(1);
            await Assert.That(messenger.CountSubscriptionsFor<TestMessage>()).IsEqualTo(1);
            var token2 = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            });
            await Assert.That(messenger.CountSubscriptionsFor<MvxSubscriberChangeMessage>()).IsEqualTo(1);
            await Assert.That(messenger.CountSubscriptionsFor<TestMessage>()).IsEqualTo(2);
            messenger.Unsubscribe<TestMessage>(token);
            await Assert.That(messenger.CountSubscriptionsFor<MvxSubscriberChangeMessage>()).IsEqualTo(1);
            await Assert.That(messenger.CountSubscriptionsFor<TestMessage>()).IsEqualTo(1);
            messenger.Unsubscribe<TestMessage>(token2);
            await Assert.That(messenger.CountSubscriptionsFor<MvxSubscriberChangeMessage>()).IsEqualTo(1);
            await Assert.That(messenger.CountSubscriptionsFor<TestMessage>()).IsEqualTo(0);
        }

        [Test]
        public async Task HasSubscriptionsForTagIsCorrect()
        {
            var testTag = "TestTag";
            var notExistingTag = "NotExistingTag";
            var messenger = new MvxMessengerHub();
            await Assert.That(messenger.HasSubscriptionsFor<MvxSubscriberChangeMessage>()).IsFalse();
            await Assert.That(messenger.HasSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag)).IsFalse();
            await Assert.That(messenger.HasSubscriptionsFor<TestMessage>()).IsFalse();
            await Assert.That(messenger.HasSubscriptionsForTag<TestMessage>(null)).IsFalse();
            await Assert.That(messenger.HasSubscriptionsForTag<TestMessage>(notExistingTag)).IsFalse();
            var changeToken = messenger.Subscribe<MvxSubscriberChangeMessage>(message => { });
            await Assert.That(messenger.HasSubscriptionsForTag<MvxSubscriberChangeMessage>(null)).IsTrue();
            await Assert.That(messenger.HasSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag)).IsFalse();
            await Assert.That(messenger.HasSubscriptionsForTag<TestMessage>(testTag)).IsFalse();
            await Assert.That(messenger.HasSubscriptionsForTag<TestMessage>(null)).IsFalse();
            await Assert.That(messenger.HasSubscriptionsForTag<TestMessage>(notExistingTag)).IsFalse();
            var token = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            }, tag: testTag);
            await Assert.That(messenger.HasSubscriptionsForTag<MvxSubscriberChangeMessage>(null)).IsTrue();
            await Assert.That(messenger.HasSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag)).IsFalse();
            await Assert.That(messenger.HasSubscriptionsForTag<TestMessage>(testTag)).IsTrue();
            await Assert.That(messenger.HasSubscriptionsForTag<TestMessage>(null)).IsFalse();
            await Assert.That(messenger.HasSubscriptionsForTag<TestMessage>(notExistingTag)).IsFalse();
            messenger.Unsubscribe<TestMessage>(token);
            await Assert.That(messenger.HasSubscriptionsForTag<MvxSubscriberChangeMessage>(null)).IsTrue();
            await Assert.That(messenger.HasSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag)).IsFalse();
            await Assert.That(messenger.HasSubscriptionsForTag<TestMessage>(testTag)).IsFalse();
            await Assert.That(messenger.HasSubscriptionsForTag<TestMessage>(null)).IsFalse();
            await Assert.That(messenger.HasSubscriptionsForTag<TestMessage>(notExistingTag)).IsFalse();
        }

        [Test]
        public async Task CountSubscriptionsForTagIsCorrect()
        {
            var testTag1 = "TestTag1";
            var testTag2 = "TestTag2";
            var notExistingTag = "NotExistingTag";
            var messenger = new MvxMessengerHub();
            await Assert.That(messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(null)).IsEqualTo(0);
            await Assert.That(messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag1)).IsEqualTo(0);
            await Assert.That(messenger.CountSubscriptionsForTag<TestMessage>(testTag1)).IsEqualTo(0);
            await Assert.That(messenger.CountSubscriptionsForTag<TestMessage>(testTag2)).IsEqualTo(0);
            await Assert.That(messenger.CountSubscriptionsForTag<TestMessage>(notExistingTag)).IsEqualTo(0);
            var changeToken = messenger.Subscribe<MvxSubscriberChangeMessage>(message => { });
            await Assert.That(messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(null)).IsEqualTo(1);
            await Assert.That(messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag1)).IsEqualTo(0);
            await Assert.That(messenger.CountSubscriptionsForTag<TestMessage>(testTag1)).IsEqualTo(0);
            await Assert.That(messenger.CountSubscriptionsForTag<TestMessage>(testTag2)).IsEqualTo(0);
            await Assert.That(messenger.CountSubscriptionsForTag<TestMessage>(notExistingTag)).IsEqualTo(0);
            var token = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            }, tag: testTag1);
            await Assert.That(messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(null)).IsEqualTo(1);
            await Assert.That(messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag1)).IsEqualTo(0);
            await Assert.That(messenger.CountSubscriptionsForTag<TestMessage>(testTag1)).IsEqualTo(1);
            await Assert.That(messenger.CountSubscriptionsForTag<TestMessage>(testTag2)).IsEqualTo(0);
            await Assert.That(messenger.CountSubscriptionsForTag<TestMessage>(notExistingTag)).IsEqualTo(0);
            var token2 = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            }, tag: testTag1);
            await Assert.That(messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(null)).IsEqualTo(1);
            await Assert.That(messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag1)).IsEqualTo(0);
            await Assert.That(messenger.CountSubscriptionsForTag<TestMessage>(testTag1)).IsEqualTo(2);
            await Assert.That(messenger.CountSubscriptionsForTag<TestMessage>(testTag2)).IsEqualTo(0);
            await Assert.That(messenger.CountSubscriptionsForTag<TestMessage>(notExistingTag)).IsEqualTo(0);
            var token3 = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            }, tag: testTag2);
            await Assert.That(messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(null)).IsEqualTo(1);
            await Assert.That(messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag1)).IsEqualTo(0);
            await Assert.That(messenger.CountSubscriptionsForTag<TestMessage>(testTag1)).IsEqualTo(2);
            await Assert.That(messenger.CountSubscriptionsForTag<TestMessage>(testTag2)).IsEqualTo(1);
            await Assert.That(messenger.CountSubscriptionsForTag<TestMessage>(notExistingTag)).IsEqualTo(0);
            messenger.Unsubscribe<TestMessage>(token);
            await Assert.That(messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(null)).IsEqualTo(1);
            await Assert.That(messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag1)).IsEqualTo(0);
            await Assert.That(messenger.CountSubscriptionsForTag<TestMessage>(testTag1)).IsEqualTo(1);
            await Assert.That(messenger.CountSubscriptionsForTag<TestMessage>(testTag2)).IsEqualTo(1);
            await Assert.That(messenger.CountSubscriptionsForTag<TestMessage>(notExistingTag)).IsEqualTo(0);
            messenger.Unsubscribe<TestMessage>(token2);
            await Assert.That(messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(null)).IsEqualTo(1);
            await Assert.That(messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag1)).IsEqualTo(0);
            await Assert.That(messenger.CountSubscriptionsForTag<TestMessage>(testTag1)).IsEqualTo(0);
            await Assert.That(messenger.CountSubscriptionsForTag<TestMessage>(testTag2)).IsEqualTo(1);
            await Assert.That(messenger.CountSubscriptionsForTag<TestMessage>(notExistingTag)).IsEqualTo(0);
            messenger.Unsubscribe<TestMessage>(token3);
            await Assert.That(messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(null)).IsEqualTo(1);
            await Assert.That(messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag1)).IsEqualTo(0);
            await Assert.That(messenger.CountSubscriptionsForTag<TestMessage>(testTag1)).IsEqualTo(0);
            await Assert.That(messenger.CountSubscriptionsForTag<TestMessage>(testTag2)).IsEqualTo(0);
            await Assert.That(messenger.CountSubscriptionsForTag<TestMessage>(notExistingTag)).IsEqualTo(0);
        }

        [Test]
        public async Task GetSubscriptionTagsIsCorrect()
        {
            var testTag1 = "TestTag1";
            var testTag2 = "TestTag2";
            var messenger = new MvxMessengerHub();
            await Assert.That(messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()).IsEmpty();
            await Assert.That(messenger.GetSubscriptionTagsFor<TestMessage>()).IsEmpty();
            var changeToken = messenger.Subscribe<MvxSubscriberChangeMessage>(message => { });
            await Assert.That(messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()).HasSingleItem();
            await Assert.That(messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()[0]).IsNull();
            await Assert.That(messenger.GetSubscriptionTagsFor<TestMessage>()).IsEmpty();
            var token = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            }, tag: testTag1);
            await Assert.That(messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()).HasSingleItem();
            await Assert.That(messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()[0]).IsNull();
            await Assert.That(messenger.GetSubscriptionTagsFor<TestMessage>()).HasSingleItem();
            await Assert.That(messenger.GetSubscriptionTagsFor<TestMessage>()[0]).IsEqualTo(testTag1);
            var token2 = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            }, tag: testTag1);
            await Assert.That(messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()).HasSingleItem();
            await Assert.That(messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()[0]).IsNull();
            await Assert.That(messenger.GetSubscriptionTagsFor<TestMessage>().Count).IsEqualTo(2);
            await Assert.That(messenger.GetSubscriptionTagsFor<TestMessage>()[0]).IsEqualTo(testTag1);
            await Assert.That(messenger.GetSubscriptionTagsFor<TestMessage>()[1]).IsEqualTo(testTag1);
            var token3 = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            }, tag: testTag2);
            await Assert.That(messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()).HasSingleItem();
            await Assert.That(messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()[0]).IsNull();
            await Assert.That(messenger.GetSubscriptionTagsFor<TestMessage>().Count).IsEqualTo(3);
            await Assert.That(messenger.GetSubscriptionTagsFor<TestMessage>().Where(x => x == testTag1).Count()).IsEqualTo(2);
            await Assert.That(messenger.GetSubscriptionTagsFor<TestMessage>().Where(x => x == testTag2)).HasSingleItem();
            messenger.Unsubscribe<TestMessage>(token);
            await Assert.That(messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()).HasSingleItem();
            await Assert.That(messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()[0]).IsNull();
            await Assert.That(messenger.GetSubscriptionTagsFor<TestMessage>().Count).IsEqualTo(2);
            await Assert.That(messenger.GetSubscriptionTagsFor<TestMessage>().Where(x => x == testTag1)).HasSingleItem();
            await Assert.That(messenger.GetSubscriptionTagsFor<TestMessage>().Where(x => x == testTag2)).HasSingleItem();
            messenger.Unsubscribe<TestMessage>(token2);
            await Assert.That(messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()).HasSingleItem();
            await Assert.That(messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()[0]).IsNull();
            await Assert.That(messenger.GetSubscriptionTagsFor<TestMessage>()).HasSingleItem();
            await Assert.That(messenger.GetSubscriptionTagsFor<TestMessage>().Where(x => x == testTag1)).IsEmpty();
            await Assert.That(messenger.GetSubscriptionTagsFor<TestMessage>().Where(x => x == testTag2)).HasSingleItem();
            messenger.Unsubscribe<TestMessage>(token3);
            await Assert.That(messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()).HasSingleItem();
            await Assert.That(messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()[0]).IsNull();
            await Assert.That(messenger.GetSubscriptionTagsFor<TestMessage>()).IsEmpty();
        }

        [Test]
        public async Task SubscribeAndUnsubscribeCauseChangeMessages()
        {
            var messenger = new MvxMessengerHub();
            MvxSubscriberChangeMessage subscriberChangeMessage = null;
            var changeToken = messenger.Subscribe<MvxSubscriberChangeMessage>(message => subscriberChangeMessage = message);
            var token = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            });
            await Assert.That(subscriberChangeMessage).IsNotNull();
            await Assert.That(subscriberChangeMessage.SubscriberCount).IsEqualTo(1);
            await Assert.That(subscriberChangeMessage.MessageType).IsEqualTo(typeof(TestMessage));
            subscriberChangeMessage = null;
            messenger.Unsubscribe<TestMessage>(token);
            await Assert.That(subscriberChangeMessage).IsNotNull();
            await Assert.That(subscriberChangeMessage.SubscriberCount).IsEqualTo(0);
            await Assert.That(subscriberChangeMessage.MessageType).IsEqualTo(typeof(TestMessage));
        }

        [Test]
        public async Task PurgeCausesChangeMessage()
        {
            var messenger = new MvxMessengerHub();
            MvxSubscriberChangeMessage subscriberChangeMessage = null;
            var changeToken = messenger.Subscribe<MvxSubscriberChangeMessage>(message => subscriberChangeMessage = message);
            CreateShortLivedSubscription(messenger);
            await Assert.That(subscriberChangeMessage).IsNotNull();
            await Assert.That(subscriberChangeMessage.SubscriberCount).IsEqualTo(1);
            await Assert.That(subscriberChangeMessage.MessageType).IsEqualTo(typeof(TestMessage));
            subscriberChangeMessage = null;
            Thread.Sleep(100);
            GC.Collect();
            GC.WaitForFullGCComplete();
            messenger.Publish(new TestMessage(this));
            Thread.Sleep(100);
            // TODO - verify that purging a weak subscription reliably publishes a change notification.
            //await Assert.That(subscriberChangeMessage).IsNotNull();
            //await Assert.That(subscriberChangeMessage.SubscriberCount).IsEqualTo(0);
            //await Assert.That(subscriberChangeMessage.MessageType).IsEqualTo(typeof(TestMessage));
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
