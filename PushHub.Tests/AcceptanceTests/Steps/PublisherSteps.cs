using System.Linq;
using System.Net;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace PushHub.Tests.AcceptanceTests.Steps
{
    [Binding]
    public class PublisherSteps
    {
        [Given(@"A hub is listening at ""(.*)""")]
        public void GivenAHubIsListeningAtHttpLocalhostPublish(string url)
        {
            var hubClient = new HubClient(url);
            ScenarioContext.Current.Add("HubClient", hubClient);
        }

        [Given(@"a publisher has registered a topic ""(.*)""")]
        public void GivenAPublisherHasRegisteredATopic(string topic)
        {
            WhenIPublishANewTopic(topic);
        }

        [Given(@"the hub contains the topic ""(.*)""")]
        public void GivenTheHubContainsTheTopic(string topic)
        {
            WhenIPublishANewTopic(topic);
        }

        [When(@"the publisher notifies the hub of updates for topic ""(.*)""")]
        public void WhenThePublisherNotifiesTheHubOfUpdatesForTopic(string topic)
        {
            var hubClient = (HubClient)ScenarioContext.Current["HubClient"];
            hubClient.Publish(topic);
        }


        [When(@"I publish a new topic ""(.*)""")]
        public void WhenIPublishANewTopic(string topic)
        {
            var hubClient = (HubClient)ScenarioContext.Current["HubClient"];
            hubClient.Publish(topic);
        }

        [Then(@"the hub will contain my new topic ""(.*)""")]
        public void ThenTheHubWillContainMyNewTopic(string topic)
        {
            var hubClient = (HubClient)ScenarioContext.Current["HubClient"];
            var topics = hubClient.ListTopics();

            Assert.That(topics.Select(t => t.Url), Contains.Item(topic));
        }

        [Then(@"the response status code should be: (.*)")]
        public void ThenTheResponseStatusCodeShouldBe(int statusCode)
        {
            var hubClient = (HubClient) ScenarioContext.Current["HubClient"];
            var response = hubClient.LastResponse;

            Assert.IsNotNull(response);
            Assert.That(response.HttpStatusCode, Is.EqualTo((HttpStatusCode)statusCode));
        }
    }
}
