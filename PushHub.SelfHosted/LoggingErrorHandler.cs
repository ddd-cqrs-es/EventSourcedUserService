﻿using System;
using Nancy;
using Nancy.ErrorHandling;
using log4net;

namespace PushHub.SelfHosted
{
    public class LoggingErrorHandler : IStatusCodeHandler
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(LoggingErrorHandler));

        public bool HandlesStatusCode(HttpStatusCode statusCode)
        {
            return statusCode == HttpStatusCode.InternalServerError;
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return true;
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            object errorObject;
            context.Items.TryGetValue(NancyEngine.ERROR_EXCEPTION, out errorObject);
            var error = errorObject as Exception;

            _logger.Error("Unhandled error", error);
        }
    }
}
