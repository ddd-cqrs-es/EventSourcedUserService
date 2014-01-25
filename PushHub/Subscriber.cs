﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PushHub
{
    public class Subscriber : IEquatable<Subscriber>
    {
        private bool _verified = false;
        private readonly string _callbackUrl;
        private readonly string _verifyToken;

        public Subscriber(string callbackUrl, string verifyToken, bool verified)
        {
            _callbackUrl = callbackUrl;
            _verifyToken = verifyToken;
            _verified = verified;
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
    }
}
