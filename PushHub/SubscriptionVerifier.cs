using System;
using System.Net;

namespace PushHub
{
    public class SubscriptionVerifier : ISubscriptionVerifier
    {
        public bool Verify(string callbackUrl, string topic, string verifyToken)
        {
            var webClient = new WebClient();
            webClient.QueryString.Add("hub.mode", "subscribe");
            webClient.QueryString.Add("hub.topic", topic);
            webClient.QueryString.Add("hub.challenge", "chanllenge");
            webClient.QueryString.Add("hub.lease_seconds", "0");
            webClient.QueryString.Add("hub.verify", verifyToken);

            try
            {
                var result = webClient.DownloadString(callbackUrl);
            }
            catch (WebException wex)
            {
                return false;
            }

            return true;
        }
    }
}
