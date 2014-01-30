﻿using System;
using System.ServiceModel.Syndication;

namespace PushHub
{
    public class Subscriber : IEquatable<Subscriber>
    {
        private bool _verified = false;
        private readonly string _callbackUrl;
        private int _position = 0;

        public Subscriber(string callbackUrl, bool verified)
        {
            _callbackUrl = callbackUrl;
            _verified = verified;
        }

        public string CallbackUrl
        {
            get { return _callbackUrl; }
        }

        public void SetFeedPosition(int position)
        {
            _position = position;
        }

        public void Verify()
        {
            _verified = true;
        }

        public bool Equals(Subscriber other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(_callbackUrl, other._callbackUrl);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Subscriber)obj);
        }

        public override int GetHashCode()
        {
            return (_callbackUrl != null ? _callbackUrl.GetHashCode() : 0);
        }

        public void NotifyContent(SyndicationFeed completeContent)
        {
            throw new NotImplementedException();
        }
    }
}
