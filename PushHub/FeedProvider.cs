using System;
using System.IO;
using System.Net;
using System.ServiceModel.Syndication;
using System.Xml;

namespace PushHub
{
    public class FeedProvider : IFeedProvider
    {
        public SyndicationFeed GetUpdatedContent(string topicUrl)
        {
            throw new NotImplementedException();
        }

        public SyndicationFeed GetCompleteContent(string topicUrl)
        {
            var webClient = new WebClient();
            webClient.Headers.Add(HttpRequestHeader.Accept, "application/atom+xml");
            var atomContent = webClient.DownloadString(topicUrl);
            using ( var sreader = new StringReader(atomContent))
            using (var reader = new XmlTextReader(sreader))
            {
                var feed = SyndicationFeed.Load(reader);
                return feed;
            }
        }
    }
}
