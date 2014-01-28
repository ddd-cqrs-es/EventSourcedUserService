namespace PushHub.Tests
{
    public class FakeSubscriptionVerifier : ISubscriptionVerifier
    {
        private readonly bool _result;

        public FakeSubscriptionVerifier(bool result)
        {
            _result = result;
        }

        public bool Verify(string callbackUrl, string topic, string verifyToken)
        {
            return _result;
        }
    }
}
