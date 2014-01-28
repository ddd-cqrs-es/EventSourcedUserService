using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace PushHub.Tests.AcceptanceTests
{
    public class HubClient
    {
        private readonly string _baseUrl;
        private RestResponseDto _lastResponse;

        public RestResponseDto LastResponse
        {
            get { return _lastResponse; }
        }

        public string HubUrl
        {
            get { return _baseUrl; }
        }

        public HubClient(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        private T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = _baseUrl;
            var response = client.Execute<T>(request);
            _lastResponse = new RestResponseDto(response);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                throw new ApplicationException(message, response.ErrorException);
            }
            return response.Data;
        }

        public void Publish(string topic)
        {
            var request = new RestRequest(Method.POST) {Resource = "publish"};
            request.AddParameter("hub.url", topic, ParameterType.GetOrPost);
            request.AddParameter("hub.mode", "publish", ParameterType.GetOrPost);

            var client = new RestClient();
            client.BaseUrl = _baseUrl;
            var response = client.Execute(request);
            _lastResponse = new RestResponseDto(response);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                throw new ApplicationException(message, response.ErrorException);
            }
        }

        public IEnumerable<Topic> ListTopics()
        {
            var request = new RestRequest(Method.GET) {Resource = "topics"};

            return Execute<List<Topic>>(request);
        }

        public IEnumerable<Subscriber> ListSubscribers()
        {
            var request = new RestRequest(Method.GET) { Resource = "subscribers" };

            return Execute<List<Subscriber>>(request);
        }

        public void ClearLastResponse()
        {
            _lastResponse = null;
        }
    }

    public class Topic
    {
        public string Url { get; set; }
    }

    public class Subscriber
    {
        public string CallbackUrl { get; set; }
    }

    public class RestResponseDto
    {
        private readonly string _content;
        private readonly IList<Parameter> _parameters;
        private readonly HttpStatusCode _httpStatusCode;
        private readonly string _statusDescription;

        public string Content
        {
            get { return _content; }
        }

        public IList<Parameter> Parameters
        {
            get { return _parameters; }
        }

        public HttpStatusCode HttpStatusCode
        {
            get { return _httpStatusCode; }
        }

        public string StatusDescription
        {
            get { return _statusDescription; }
        }

        public RestResponseDto(IRestResponse restResponse)
        {
            _content = restResponse.Content;
            _parameters = restResponse.Headers;
            _httpStatusCode = restResponse.StatusCode;
            _statusDescription = restResponse.StatusDescription;
        }
    }
}
