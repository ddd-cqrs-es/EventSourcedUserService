using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;

namespace PushHub.Tests
{
    public class FakeFeedProvider : IFeedProvider
    {
        private readonly SyndicationFeed _feed;

        public FakeFeedProvider(SyndicationFeed feed)
        {
            _feed = feed;
        }

        public SyndicationFeed GetCompleteContent(string topicUrl)
        {
            return _feed;
        }

        public SyndicationFeed GetUpdatedContent(string topicUrl)
        {
            return _feed;
        }
    }
}
