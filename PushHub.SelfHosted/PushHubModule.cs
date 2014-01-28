using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Nancy;
using PushHub.SelfHosted.Models;

namespace PushHub.SelfHosted
{
    public class PushHubModule : NancyModule
    {
        private readonly Hub _hub;

        public PushHubModule(Hub hub)
        {
            _hub = hub;

            //define routes:

            //GET topics: gets a list of current topics
            Get["/topics"] = _ => _hub.GetTopics().Select(t => new Topic{Url = t});

            Get["/subscribers"] = _ => _hub.Subscribers().Select(s => new Models.Subscriber{CallbackUrl = s.CallbackUrl});

            //POST: publishers hit this endpoint with new content
            //Condition: form content must contain: hub.mode and hub.url
            Post["/publish", context =>
                ((Dictionary<string, object>.KeyCollection) context.Request.Form.Keys)
                    .Contains("hub.url") &&
                ((Dictionary<string, object>.KeyCollection) context.Request.Form.Keys)
                    .Contains("hub.mode")] = _ =>
                        {

                            if (Context.Request.Form["hub.mode"].Value != "publish")
                                return HttpStatusCode.BadRequest;

                            string topic = Context.Request.Form["hub.url"].Value;
                            if (_hub.GetTopics().Contains(topic))
                            {
                                _hub.Update(topic);
                            }
                            else
                            {
                                _hub.NewTopic(topic);
                            }

                            return HttpStatusCode.NoContent;
                        };

            //POST: subscribers hit this endpoint to subscribe to a topic
            //Condition: form content must contain: hub.callback, hub.topic, hub.mode, hub.verify, hub.verify_token
            Post["/subscribe", context =>
                               ((Dictionary<string, object>.KeyCollection) context.Request.Form.Keys)
                                   .Contains("hub.callback") &&
                               ((Dictionary<string, object>.KeyCollection) context.Request.Form.Keys)
                                   .Contains("hub.topic") &&
                               ((Dictionary<string, object>.KeyCollection) context.Request.Form.Keys)
                                   .Contains("hub.mode") &&
                               ((Dictionary<string, object>.KeyCollection) context.Request.Form.Keys)
                                   .Contains("hub.verify") &&
                               ((Dictionary<string, object>.KeyCollection) context.Request.Form.Keys)
                                   .Contains("hub.verify_token")] = _ =>
                                       {
                                           //TODO: handle hub.secret -> hub must send a secret to the subscriber. The subscriber will
                                           //send the secret back to the hub upon verification. Hub must verify the secret.
                                           string hubMode = Context.Request.Form["hub.mode"].Value;
                                           if (hubMode.ToLower() != "subscribe")
                                               throw new Exception("Expected hub.mode = subscribe");

                                           string topic = Context.Request.Form["hub.topic"].Value;
                                           string callbackUrl = Context.Request.Form["hub.callback"].Value;
                                           string verifyType = Context.Request.Form["hub.verify"].Value;
                                           string verifyToken = Context.Request.Form["hub.verify_token"].Value;

                                           if(string.IsNullOrEmpty(topic))
                                               throw new Exception("hub.topic was null");

                                           if(string.IsNullOrEmpty(callbackUrl))
                                               throw  new Exception("hub.callback was null");

                                           if(string.IsNullOrEmpty(verifyType))
                                               throw new Exception("hub.verify was null");

                                           if(string.IsNullOrEmpty(verifyToken))
                                               throw new Exception("hub.verify_token was null");

                                           if(verifyType != "sync")
                                               throw new Exception("Only supprts hub.verify = sync");

                                           _hub.NewSubscriberSynchronous(topic, callbackUrl, verifyToken);

                                           return HttpStatusCode.NoContent;
                                       };
        }
    }
}
