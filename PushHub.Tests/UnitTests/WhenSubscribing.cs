using System.Collections.Generic;
using NUnit.Framework;

namespace PushHub.Tests.UnitTests
{
    [TestFixture]
    public class WhenSubscribing
    {
        [Test]
        public void When_client_subscribes_for_the_first_time()
        {
            var topicUrl = "http://localhost:8080/topic1";
            var callBackUrl = "http://host/callback";
            var verifyToken = "verifytoken";
            var hub = new Hub(new FakeSubscriptionVerifier(true), new FakeFeedProvider(null));

            hub.NewTopic(topicUrl);
            hub.NewSubscriberSynchronous(topicUrl, callBackUrl, verifyToken);

            CollectionAssert.Contains(hub.Subscribers(), new Subscriber(callBackUrl, true));
        }

        [Test]
        public void When_client_subscribes_for_a_second_time()
        {
            var topicUrl = "http://localhost:8080/topic1";
            var callBackUrl = "http://host/callback";
            var verifyToken = "verifytoken";
            var hub = new Hub(new FakeSubscriptionVerifier(true), new FakeFeedProvider(null));

            hub.NewTopic(topicUrl);
            hub.NewSubscriberSynchronous(topicUrl, callBackUrl, verifyToken);
            hub.NewSubscriberSynchronous(topicUrl, callBackUrl, verifyToken);

            CollectionAssert.Contains(hub.Subscribers(), new Subscriber(callBackUrl, true));
        }

        [Test]
        public void When_client_subscribes_for_a_non_existant_topic()
        {
            var topicUrl = "http://localhost:8080/topic1";
            var callBackUrl = "http://host/callback";
            var verifyToken = "verifytoken";
            var hub = new Hub(new FakeSubscriptionVerifier(true), new FakeFeedProvider(null));

            hub.NewTopic(topicUrl);
            Assert.Throws<KeyNotFoundException>(() => hub.NewSubscriberSynchronous("wrongtopic", callBackUrl, verifyToken));
        }
    }
}
