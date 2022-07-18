// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading;
using MvvmCross.Plugin.Messenger;
using MvvmCross.Tests;
using Xunit;

namespace MvvmCross.Plugin.Messenger.UnitTest
{
    public class MessengerHubTest : IClassFixture<MvxTestFixture>
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


        [Fact]
        public void SubscribeAndPublishAllowsMessageToBeReceived()
        {
            var messenger = new MvxMessengerHub();
            var message = new TestMessage(this);

            var messageReceived = false;
            messenger.Subscribe<TestMessage>(m =>
            {
                Assert.Equal(message, m);
                Assert.Equal(this, m.Sender);
                messageReceived = true;
            });

            messenger.Publish(message);

            Assert.True(messageReceived);
        }

        [Fact]
        public void MultipleSubscribeAndPublishAllowsMessageToBeReceived()
        {
            var messenger = new MvxMessengerHub();
            var message = new TestMessage(this);
            var otherMessage = new OtherTestMessage(this);

            var messageReceived = 0;
            messenger.Subscribe<TestMessage>(m =>
            {
                Assert.Equal(message, m);
                Assert.Equal(this, m.Sender);
                messageReceived++;
            });

            var otherMessageReceived = 0;
            messenger.Subscribe<OtherTestMessage>(m =>
            {
                Assert.Equal(otherMessage, m);
                Assert.Equal(this, m.Sender);
                otherMessageReceived++;
            });

            messenger.Publish(otherMessage);
            Assert.Equal(0, messageReceived);
            Assert.Equal(1, otherMessageReceived);

            messenger.Publish(message);
            Assert.Equal(1, messageReceived);
            Assert.Equal(1, otherMessageReceived);

            messenger.Publish(message);
            Assert.Equal(2, messageReceived);
            Assert.Equal(1, otherMessageReceived);

            messenger.Publish(message);
            Assert.Equal(3, messageReceived);
            Assert.Equal(1, otherMessageReceived);

            messenger.Publish(otherMessage);
            Assert.Equal(3, messageReceived);
            Assert.Equal(2, otherMessageReceived);
        }

        [Fact]
        public void UnsubscribePreventsMessagesBeingReceived()
        {
            var messenger = new MvxMessengerHub();
            Action<TestMessage> action = _ => Assert.True(false, "This event should not fire!");

            var id = messenger.Subscribe(action);
            messenger.Unsubscribe<TestMessage>(id);
            messenger.Publish(new TestMessage(this));
        }

        [Fact]
        public void DisposeTokenPreventsMessagesBeingReceived()
        {
            var messenger = new MvxMessengerHub();
            Action<TestMessage> action = _ => Assert.True(false, "This event should not fire!");

            var id = messenger.Subscribe(action);
            id.Dispose();
            messenger.Publish(new TestMessage(this));
        }

        [Fact]
        public void NullSenderCausesException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var message = new TestMessage(null);
            });
        }

        [Fact]
        public void NullSubscribeCausesException()
        {
            var messenger = new MvxMessengerHub();
            Assert.Throws<ArgumentNullException>(() =>
            {
                messenger.Subscribe<TestMessage>(null);
            });
        }

        [Fact]
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

        [Fact]
        public void NullPublishCausesException()
        {
            var messenger = new MvxMessengerHub();
            Assert.Throws<ArgumentNullException>(() =>
            {
                messenger.Publish<TestMessage>(null);
            });
        }

        [Fact]
        public void HasSubscriptionsForIsCorrect()
        {
            var messenger = new MvxMessengerHub();
            Assert.False(messenger.HasSubscriptionsFor<MvxSubscriberChangeMessage>());
            Assert.False(messenger.HasSubscriptionsFor<TestMessage>());
            var changeToken = messenger.Subscribe<MvxSubscriberChangeMessage>(message => { });
            Assert.True(messenger.HasSubscriptionsFor<MvxSubscriberChangeMessage>());
            Assert.False(messenger.HasSubscriptionsFor<TestMessage>());
            var token = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            });
            Assert.True(messenger.HasSubscriptionsFor<MvxSubscriberChangeMessage>());
            Assert.True(messenger.HasSubscriptionsFor<TestMessage>());
            messenger.Unsubscribe<TestMessage>(token);
            Assert.True(messenger.HasSubscriptionsFor<MvxSubscriberChangeMessage>());
            Assert.False(messenger.HasSubscriptionsFor<TestMessage>());
        }

        [Fact]
        public void CountSubscriptionsForIsCorrect()
        {
            var messenger = new MvxMessengerHub();
            Assert.False(messenger.HasSubscriptionsFor<MvxSubscriberChangeMessage>());
            Assert.False(messenger.HasSubscriptionsFor<TestMessage>());
            var changeToken = messenger.Subscribe<MvxSubscriberChangeMessage>(message => { });
            Assert.Equal(1, messenger.CountSubscriptionsFor<MvxSubscriberChangeMessage>());
            Assert.Equal(0, messenger.CountSubscriptionsFor<TestMessage>());
            var token = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            });
            Assert.Equal(1, messenger.CountSubscriptionsFor<MvxSubscriberChangeMessage>());
            Assert.Equal(1, messenger.CountSubscriptionsFor<TestMessage>());
            var token2 = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            });
            Assert.Equal(1, messenger.CountSubscriptionsFor<MvxSubscriberChangeMessage>());
            Assert.Equal(2, messenger.CountSubscriptionsFor<TestMessage>());
            messenger.Unsubscribe<TestMessage>(token);
            Assert.Equal(1, messenger.CountSubscriptionsFor<MvxSubscriberChangeMessage>());
            Assert.Equal(1, messenger.CountSubscriptionsFor<TestMessage>());
            messenger.Unsubscribe<TestMessage>(token2);
            Assert.Equal(1, messenger.CountSubscriptionsFor<MvxSubscriberChangeMessage>());
            Assert.Equal(0, messenger.CountSubscriptionsFor<TestMessage>());
        }

        [Fact]
        public void HasSubscriptionsForTagIsCorrect()
        {
            var testTag = "TestTag";
            var notExistingTag = "NotExistingTag";
            var messenger = new MvxMessengerHub();
            Assert.False(messenger.HasSubscriptionsFor<MvxSubscriberChangeMessage>());
            Assert.False(messenger.HasSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag));
            Assert.False(messenger.HasSubscriptionsFor<TestMessage>());
            Assert.False(messenger.HasSubscriptionsForTag<TestMessage>(null));
            Assert.False(messenger.HasSubscriptionsForTag<TestMessage>(notExistingTag));
            var changeToken = messenger.Subscribe<MvxSubscriberChangeMessage>(message => { });
            Assert.True(messenger.HasSubscriptionsForTag<MvxSubscriberChangeMessage>(null));
            Assert.False(messenger.HasSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag));
            Assert.False(messenger.HasSubscriptionsForTag<TestMessage>(testTag));
            Assert.False(messenger.HasSubscriptionsForTag<TestMessage>(null));
            Assert.False(messenger.HasSubscriptionsForTag<TestMessage>(notExistingTag));
            var token = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            }, tag: testTag);
            Assert.True(messenger.HasSubscriptionsForTag<MvxSubscriberChangeMessage>(null));
            Assert.False(messenger.HasSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag));
            Assert.True(messenger.HasSubscriptionsForTag<TestMessage>(testTag));
            Assert.False(messenger.HasSubscriptionsForTag<TestMessage>(null));
            Assert.False(messenger.HasSubscriptionsForTag<TestMessage>(notExistingTag));
            messenger.Unsubscribe<TestMessage>(token);
            Assert.True(messenger.HasSubscriptionsForTag<MvxSubscriberChangeMessage>(null));
            Assert.False(messenger.HasSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag));
            Assert.False(messenger.HasSubscriptionsForTag<TestMessage>(testTag));
            Assert.False(messenger.HasSubscriptionsForTag<TestMessage>(null));
            Assert.False(messenger.HasSubscriptionsForTag<TestMessage>(notExistingTag));
        }

        [Fact]
        public void CountSubscriptionsForTagIsCorrect()
        {
            var testTag1 = "TestTag1";
            var testTag2 = "TestTag2";
            var notExistingTag = "NotExistingTag";
            var messenger = new MvxMessengerHub();
            Assert.Equal(0, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(null));
            Assert.Equal(0, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag1));
            Assert.Equal(0, messenger.CountSubscriptionsForTag<TestMessage>(testTag1));
            Assert.Equal(0, messenger.CountSubscriptionsForTag<TestMessage>(testTag2));
            Assert.Equal(0, messenger.CountSubscriptionsForTag<TestMessage>(notExistingTag));
            var changeToken = messenger.Subscribe<MvxSubscriberChangeMessage>(message => { });
            Assert.Equal(1, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(null));
            Assert.Equal(0, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag1));
            Assert.Equal(0, messenger.CountSubscriptionsForTag<TestMessage>(testTag1));
            Assert.Equal(0, messenger.CountSubscriptionsForTag<TestMessage>(testTag2));
            Assert.Equal(0, messenger.CountSubscriptionsForTag<TestMessage>(notExistingTag));
            var token = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            }, tag: testTag1);
            Assert.Equal(1, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(null));
            Assert.Equal(0, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag1));
            Assert.Equal(1, messenger.CountSubscriptionsForTag<TestMessage>(testTag1));
            Assert.Equal(0, messenger.CountSubscriptionsForTag<TestMessage>(testTag2));
            Assert.Equal(0, messenger.CountSubscriptionsForTag<TestMessage>(notExistingTag));
            var token2 = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            }, tag: testTag1);
            Assert.Equal(1, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(null));
            Assert.Equal(0, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag1));
            Assert.Equal(2, messenger.CountSubscriptionsForTag<TestMessage>(testTag1));
            Assert.Equal(0, messenger.CountSubscriptionsForTag<TestMessage>(testTag2));
            Assert.Equal(0, messenger.CountSubscriptionsForTag<TestMessage>(notExistingTag));
            var token3 = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            }, tag: testTag2);
            Assert.Equal(1, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(null));
            Assert.Equal(0, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag1));
            Assert.Equal(2, messenger.CountSubscriptionsForTag<TestMessage>(testTag1));
            Assert.Equal(1, messenger.CountSubscriptionsForTag<TestMessage>(testTag2));
            Assert.Equal(0, messenger.CountSubscriptionsForTag<TestMessage>(notExistingTag));
            messenger.Unsubscribe<TestMessage>(token);
            Assert.Equal(1, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(null));
            Assert.Equal(0, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag1));
            Assert.Equal(1, messenger.CountSubscriptionsForTag<TestMessage>(testTag1));
            Assert.Equal(1, messenger.CountSubscriptionsForTag<TestMessage>(testTag2));
            Assert.Equal(0, messenger.CountSubscriptionsForTag<TestMessage>(notExistingTag));
            messenger.Unsubscribe<TestMessage>(token2);
            Assert.Equal(1, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(null));
            Assert.Equal(0, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag1));
            Assert.Equal(0, messenger.CountSubscriptionsForTag<TestMessage>(testTag1));
            Assert.Equal(1, messenger.CountSubscriptionsForTag<TestMessage>(testTag2));
            Assert.Equal(0, messenger.CountSubscriptionsForTag<TestMessage>(notExistingTag));
            messenger.Unsubscribe<TestMessage>(token3);
            Assert.Equal(1, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(null));
            Assert.Equal(0, messenger.CountSubscriptionsForTag<MvxSubscriberChangeMessage>(testTag1));
            Assert.Equal(0, messenger.CountSubscriptionsForTag<TestMessage>(testTag1));
            Assert.Equal(0, messenger.CountSubscriptionsForTag<TestMessage>(testTag2));
            Assert.Equal(0, messenger.CountSubscriptionsForTag<TestMessage>(notExistingTag));
        }

        [Fact]
        public void GetSubscriptionTagsIsCorrect()
        {
            var testTag1 = "TestTag1";
            var testTag2 = "TestTag2";
            var messenger = new MvxMessengerHub();
            Assert.Empty(messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>());
            Assert.Empty(messenger.GetSubscriptionTagsFor<TestMessage>());
            var changeToken = messenger.Subscribe<MvxSubscriberChangeMessage>(message => { });
            Assert.Equal(1, messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>().Count);
            Assert.Null(messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()[0]);
            Assert.Empty(messenger.GetSubscriptionTagsFor<TestMessage>());
            var token = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            }, tag: testTag1);
            Assert.Equal(1, messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>().Count);
            Assert.Null(messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()[0]);
            Assert.Equal(1, messenger.GetSubscriptionTagsFor<TestMessage>().Count);
            Assert.Equal(testTag1, messenger.GetSubscriptionTagsFor<TestMessage>()[0]);
            var token2 = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            }, tag: testTag1);
            Assert.Equal(1, messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>().Count);
            Assert.Null(messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()[0]);
            Assert.Equal(2, messenger.GetSubscriptionTagsFor<TestMessage>().Count);
            Assert.Equal(testTag1, messenger.GetSubscriptionTagsFor<TestMessage>()[0]);
            Assert.Equal(testTag1, messenger.GetSubscriptionTagsFor<TestMessage>()[1]);
            var token3 = messenger.Subscribe<TestMessage>(m =>
            {
                // stuff
            }, tag: testTag2);
            Assert.Equal(1, messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>().Count);
            Assert.Null(messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()[0]);
            Assert.Equal(3, messenger.GetSubscriptionTagsFor<TestMessage>().Count);
            Assert.Equal(2, messenger.GetSubscriptionTagsFor<TestMessage>().Where(x => x == testTag1).Count());
            Assert.Single(messenger.GetSubscriptionTagsFor<TestMessage>().Where(x => x == testTag2));
            messenger.Unsubscribe<TestMessage>(token);
            Assert.Equal(1, messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>().Count);
            Assert.Null(messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()[0]);
            Assert.Equal(2, messenger.GetSubscriptionTagsFor<TestMessage>().Count);
            Assert.Single(messenger.GetSubscriptionTagsFor<TestMessage>().Where(x => x == testTag1));
            Assert.Single(messenger.GetSubscriptionTagsFor<TestMessage>().Where(x => x == testTag2));
            messenger.Unsubscribe<TestMessage>(token2);
            Assert.Equal(1, messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>().Count);
            Assert.Null(messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()[0]);
            Assert.Equal(1, messenger.GetSubscriptionTagsFor<TestMessage>().Count);
            Assert.Empty(messenger.GetSubscriptionTagsFor<TestMessage>().Where(x => x == testTag1));
            Assert.Single(messenger.GetSubscriptionTagsFor<TestMessage>().Where(x => x == testTag2));
            messenger.Unsubscribe<TestMessage>(token3);
            Assert.Equal(1, messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>().Count);
            Assert.Null(messenger.GetSubscriptionTagsFor<MvxSubscriberChangeMessage>()[0]);
            Assert.Equal(0, messenger.GetSubscriptionTagsFor<TestMessage>().Count);
        }

        [Fact]
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
            Assert.Equal(1, subscriberChangeMessage.SubscriberCount);
            Assert.Equal(typeof(TestMessage), subscriberChangeMessage.MessageType);
            subscriberChangeMessage = null;
            messenger.Unsubscribe<TestMessage>(token);
            Assert.NotNull(subscriberChangeMessage);
            Assert.Equal(0, subscriberChangeMessage.SubscriberCount);
            Assert.Equal(typeof(TestMessage), subscriberChangeMessage.MessageType);
        }

        [Fact]
        public void PurgeCausesChangeMessage()
        {
            var messenger = new MvxMessengerHub();
            MvxSubscriberChangeMessage subscriberChangeMessage = null;
            var changeToken = messenger.Subscribe<MvxSubscriberChangeMessage>(message => subscriberChangeMessage = message);
            CreateShortLivedSubscription(messenger);
            Assert.NotNull(subscriberChangeMessage);
            Assert.Equal(1, subscriberChangeMessage.SubscriberCount);
            Assert.Equal(typeof(TestMessage), subscriberChangeMessage.MessageType);
            subscriberChangeMessage = null;
            Thread.Sleep(100);
            GC.Collect();
            GC.WaitForFullGCComplete();
            messenger.Publish(new TestMessage(this));
            Thread.Sleep(100);
            // TODO - figure out why this test fails in NUnit console runner, but not through VS Test Execution
            //Assert.NotNull(subscriberChangeMessage);
            //Assert.Equal(0, subscriberChangeMessage.SubscriberCount);
            //Assert.Equal(typeof(TestMessage), subscriberChangeMessage.MessageType);
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
