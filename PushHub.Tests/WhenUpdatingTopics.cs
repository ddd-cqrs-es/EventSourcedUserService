using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PushHub.Tests
{
    [TestFixture]
    public class WhenUpdatingTopics
    {
        [Test]
        [Ignore]
        public void When_updating_existing_topic()
        {
            var topicUrl = "http://localhost/topic1";
            var hub = new Hub(new FakeSubscriptionVerifier(true), new FakeFeedProvider(null));
            hub.NewSubscriberSynchronous(topicUrl, "", "verifytoken");


            hub.Update(topicUrl);


        }
    }
}
