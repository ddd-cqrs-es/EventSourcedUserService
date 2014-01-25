﻿using System;
using System.Diagnostics;
using System.Xml;
using PushSubscriber;

namespace PubSubTester
{
    class Program
    {
        const string FeedToSubscribe = "http://192.168.1.80:2113/streams/User-0d153d8f-0623-46a6-814e-e4df0b87f68c";
        const string HubUrl = PuSHSubscriber.DefaultSubscribeHub;

        const string CallbackUrl = "http://192.168.1.80:8080";
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
                    args.Feed.SaveAsAtom10(writer);
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
