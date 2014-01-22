using System;
using System.Diagnostics;
using System.Xml;
using PushSubscriber;

namespace PubSubTester
{
    class Program
    {
        const string FeedToSubscribe = "feed url here";
        const string HubUrl = PuSHSubscriber.DefaultSubscribeHub;

        const string CallbackUrl = "your callback url here";
        static void Main(string[] args)
        {
            // So trace output will go to the console.
            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Debug.AutoFlush = true;
            var callback = new PushSubscriberCallback("http://+:8080/");
            try
            {
                callback.Start();
                callback.PushPost += callback_PushPost;
                callback.PushVerify += callback_PushVerify;
                // Subscribe to a feed
                Console.WriteLine("Subscribing to {0}", FeedToSubscribe);
                var statusCode = PuSHSubscriber.Subscribe(
                    HubUrl,
                    CallbackUrl,
                    FeedToSubscribe,
                    PushVerifyType.Sync,
                    0,
                    "xyzzy",
                    null);
                Console.WriteLine("Status code = " + statusCode);
                Console.WriteLine("Listening for connections from hub.");
                Console.WriteLine("Press Enter to exit program.");
                Console.ReadLine();
                // Unsubscribe
                Console.WriteLine("Unsubscribing...");
                statusCode = PuSHSubscriber.Unsubscribe(
                    HubUrl,
                    CallbackUrl,
                    FeedToSubscribe,
                    PushVerifyType.Sync,
                    "xyzzy");
                Console.WriteLine("Return value = " + statusCode);
            }
            finally
            {
                callback.Stop();
                callback.Dispose();
            }
            Debug.Flush();
        }
        const string FeedBaseName = "feed";
        const string FeedExtension = ".xml";
        static void callback_PushPost(object sender, PushPostEventArgs args)
        {
            Console.WriteLine("{0} - Received update from hub!", DateTime.Now);
            try
            {
                // Save the update to file.
                string timestamp = DateTime.Now.ToString("yyyyMMdd_hhmm");
                string saveFilename = FeedBaseName + "_" + timestamp + FeedExtension;
                Console.WriteLine("Writing feed to {0}", saveFilename);
                using (var writer = XmlWriter.Create(saveFilename))
                {
                    args.Feed.SaveAsRss20(writer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception!\r\n{0}", ex);
            }
            Console.WriteLine("Done");
        }
        static void callback_PushVerify(object sender, PushVerifyEventArgs args)
        {
            Console.WriteLine("{0} - Received verify message from hub.", DateTime.Now);
            // Verify all requests.
            args.Allow = true;
        }
    }
}
