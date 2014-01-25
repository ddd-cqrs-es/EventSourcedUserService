using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;

namespace PushHub
{
    public interface IFeedProvider
    {
        /// <summary>
        /// Returns only that content that has changed
        /// </summary>
        /// <param name="topicUrl"></param>
        SyndicationFeed GetUpdatedContent(string topicUrl);

        SyndicationFeed GetCompleteContent(string topicUrl);
    }
}
