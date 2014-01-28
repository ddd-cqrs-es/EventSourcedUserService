using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using log4net;

namespace PushHub
{
    public class Hub
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Hub));
        private readonly ConcurrentDictionary<string, List<Subscriber>> _topics = 
            new ConcurrentDictionary<string, List<Subscriber>>();

        private readonly ISubscriptionVerifier _subscriptionVerifier;
        private readonly IFeedProvider _feedProvider;

        public Hub(ISubscriptionVerifier subscriptionVerifier, IFeedProvider feedProvider)
        {
            _subscriptionVerifier = subscriptionVerifier;
            _feedProvider = feedProvider;
        }

        public void NewTopic(string topicUrl)
        {
            if (!_topics.ContainsKey(topicUrl))
                if(!_topics.TryAdd(topicUrl, new List<Subscriber>()))
                    Logger.ErrorFormat("Could not add new topic '{0}' to topics collection", topicUrl);
        }

        public void NewSubscriberSynchronous(string topicUrl, string callBackUrl, string verifyToken)
        {
            if (!_topics.ContainsKey(topicUrl))
                throw new KeyNotFoundException(string.Format("Topic '{0}' is not registered.", topicUrl));

            Logger.InfoFormat("New subscriber [{0}] for topic '{1}'", callBackUrl, topicUrl);

            //do verification of new subscriber
            if(!_subscriptionVerifier.Verify(callBackUrl, topicUrl, verifyToken))
                Logger.InfoFormat("Subscription from [{0}] was not verified.", callBackUrl);

            _topics.AddOrUpdate(topicUrl, s => new List<Subscriber> { new Subscriber(callBackUrl, verifyToken, true) },
                    (s, list) =>
                    {
                        list.Add(new Subscriber(callBackUrl, verifyToken, true));
                        return list;
                    });
        }

        public void Update(string topicUrl)
        {
            //go get new content from source (the topic url)
            var completeContent = _feedProvider.GetCompleteContent(topicUrl);
            //TODO: diff the content
            var webClient = new WebClient();

//            var atom10FeedFormatter = completeContent.GetAtom10Formatter();
//            using (var sWriter = new StringWriter())
//            using (var xWriter = new XmlTextWriter(sWriter))
//            {
//                atom10FeedFormatter.WriteTo(xWriter);
//                //send diff to the subscribers
//                foreach (var subscriber in _topics[topicUrl])
//                {
//                    webClient.UploadString(subscriber.CallbackUrl, sWriter.GetStringBuilder().ToString());
//                }
//            }

            var xmlReader = XmlReader.Create(topicUrl);
            var feed = SyndicationFeed.Load(xmlReader);
            //feed.SaveAsAtom10();
            using(var stream = new StringWriter())
            using (var writer = new XmlTextWriter(stream))
            {
                writer.WriteNode(xmlReader, false);
                foreach (var subscriber in _topics[topicUrl])
                {
                    webClient.UploadString(subscriber.CallbackUrl, stream.GetStringBuilder().ToString());
                }
            }
        }

        public IEnumerable<string> GetTopics()
        {
            return _topics.Keys;
        }

        public IEnumerable<Subscriber> Subscribers()
        {
            return new ReadOnlyCollection<Subscriber>(_topics.SelectMany(pair => pair.Value).ToList());
        }
    }

    public enum PushVerifyType
    {
        Async,
        Sync
    }

    public enum PushSubscriberMode
    {
        Subscribe,
        Unsubscribe
    }
}
