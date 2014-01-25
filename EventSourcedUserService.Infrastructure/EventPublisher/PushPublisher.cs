using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

//Taken from: http://www.informit.com/guides/content.aspx?g=dotnet&seqNum=802

namespace UserService.Infrastructure.EventPublisher
{
    public static class PushPublisher
    {
        public const string DefaultHubUrl = "http://192.168.1.86:4567";

        /// <summary>

        /// Publish a topic on a PubSubHubbub-hub, notifies the hub that there's an update.
        /// </summary>
        /// <param name="hubURL">URL to the PubSubHubbub-hub</param>
        /// <param name="topicURL">URL to the topic</param>
        public static void Publish(string hubUrl, string topicUrl)
        {
            if (String.IsNullOrEmpty(hubUrl))
            {
                throw new ArgumentException("Null or empty hubUrl", "hubUrl");
            }
            if (String.IsNullOrEmpty(topicUrl))
            {
                throw new ArgumentException("Null or empty topicUrl", "topicUrl");
            }
            try
            {
                string postDataStr = "hub.mode=publish&hub.url=" + HttpUtility.UrlEncode(topicUrl);
                byte[] postData = Encoding.UTF8.GetBytes(postDataStr);
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(hubUrl);
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/x-www-form-urlencoded";
                httpRequest.ContentLength = postData.Length;
                Stream requestStream = httpRequest.GetRequestStream();
                requestStream.Write(postData, 0, postData.Length);
                requestStream.Flush();
                requestStream.Close();
                using (HttpWebResponse webResponse = (HttpWebResponse)httpRequest.GetResponse())
                {
                    if (httpRequest.HaveResponse)
                    {
                        Stream responseStream = webResponse.GetResponseStream();
                        StreamReader responseReader = new System.IO.StreamReader(responseStream, Encoding.UTF8);
                        responseReader.ReadToEnd();
                        if (webResponse.StatusCode != HttpStatusCode.NoContent)
                        {
                            throw new ApplicationException("Received unexpected statusCode from hub: '" +
                                webResponse.StatusCode.ToString() + "' (should be 204 'No Content')");
                        }
                    }
                    else
                    {
                        throw new ApplicationException("No response from hub.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error publishing topicURL '" +
                    topicUrl + "' to hub '" + hubUrl + "'", ex);
            }
        }
    }
}
