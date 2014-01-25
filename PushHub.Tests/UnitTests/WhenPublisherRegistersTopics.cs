using NUnit.Framework;

namespace PushHub.Tests.UnitTests
{
    [TestFixture]
    public class WhenPublisherRegistersTopics
    {
        [Test]
        public void When_a_publisher_registers_a_new_topic()
        {
            var hubTopic = "http://localhost:8080/topic1"; //the url of the topic
            var hub = new Hub(new FakeSubscriptionVerifier(true), new FakeFeedProvider(null));

            hub.NewTopic(hubTopic);

            Assert.That(hub.GetTopics(), Has.Count.EqualTo(1));
            Assert.That(hub.GetTopics(), Contains.Item(hubTopic));
        }

        [Test]
        public void When_a_publisher_registers_an_existing_topic()
        {
            var hubTopic = "http://localhost:8080/topic1"; //the url of the topic
            var hub = new Hub(new FakeSubscriptionVerifier(true), new FakeFeedProvider(null));

            hub.NewTopic(hubTopic);
            hub.NewTopic(hubTopic);

            Assert.That(hub.GetTopics(), Has.Count.EqualTo(1));
            Assert.That(hub.GetTopics(), Contains.Item(hubTopic));
        }
    }
}
