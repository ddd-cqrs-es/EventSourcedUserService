using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using NUnit.Framework;
using PushSubscriber;
using TechTalk.SpecFlow;

namespace PushHub.Tests.AcceptanceTests.Steps
{
    [Binding]
    public class SubscriberSteps
    {
        private bool _verifyWasCalled = false;
        private SyndicationFeed _updatedFeedContent;


        [Given(@"a subscriber callback is configured to listen at ""(.*)"" with a prefix of ""(.*)""")]
        public void GivenASubscriberCallbackIsConfiguredToListenAtWithAPrefixOf(string callbackUrl, string prefix)
        {
            var subscriberCallback = new PushSubscriberCallback(prefix);
            subscriberCallback.PushVerify += (sender, args) =>
            {
                _verifyWasCalled = true;
                args.Allow = true;
            };
            subscriberCallback.PushPost += (sender, args) =>
                {
                    _updatedFeedContent = args.Feed;
                };
            subscriberCallback.Start();

            ScenarioContext.Current.Add("SubscriberCallback", subscriberCallback);
            ScenarioContext.Current.Add("SubscriberCallbackUrl", callbackUrl);
        }

        [Given(@"the subscriber is listening for the topic ""(.*)""")]
        public void GivenTheSubscriberIsListeningForTheTopic(string topic)
        {
            WhenISubscribeForTheTopic(topic);
        }

        [When(@"I subscribe for the topic ""(.*)""")]
        public void WhenISubscribeForTheTopic(string topic)
        {
            var hubUrl = ((HubClient)ScenarioContext.Current["HubClient"]).HubUrl;
            var subscriberCallBackUrl = ScenarioContext.Current["SubscriberCallbackUrl"].ToString();
            var responseStatusCode = PuSHSubscriber.Subscribe(hubUrl + "/subscribe", subscriberCallBackUrl, topic,
                                                              PushSubscriber.PushVerifyType.Sync, 0, "verifytoken", null);
            ScenarioContext.Current.Add("SubscribeHttpResponseCode", responseStatusCode);
        }

        [Then(@"subscriber receives updates from the hub")]
        public void ThenSubscriberReceivesUpdatesFromTheHub()
        {
            Assert.IsNotNull(_updatedFeedContent);
        }


        [Then(@"the subscription response status code should be: (.*)")]
        public void ThenTheSubscriptionResponseStatusCodeShouldBe(int theExpectedResponseCode)
        {
            var theActualresponseCode = (HttpStatusCode)ScenarioContext.Current["SubscribeHttpResponseCode"];

            Assert.IsNotNull(theActualresponseCode);
            Assert.That(theActualresponseCode, Is.EqualTo((HttpStatusCode)theExpectedResponseCode));
        }

        [Then(@"the hub calls me back to verify my subscription")]
        public void ThenTheHubCallsMeBackToVerifyMySubscription()
        {
            Assert.IsTrue(_verifyWasCalled);
        }

        [Then(@"the hub contains a subscription with a callback url of ""(.*)""")]
        public void ThenTheHubContainsASubscriptionWithACallbackUrlOf(string subscriberCallbackUrl)
        {
            var hubClient = (HubClient) ScenarioContext.Current["HubClient"];
            var subscribers = hubClient.ListSubscribers();

            Assert.IsNotNull(subscribers);
            Assert.That(subscribers.Select(s => s.CallbackUrl), Contains.Item(subscriberCallbackUrl));
        }


    }
}
