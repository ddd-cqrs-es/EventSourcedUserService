namespace PushHub.Tests
{
    public class FakeSubscriptionVerifier : ISubscriptionVerifier
    {
        private readonly bool _result;

        public FakeSubscriptionVerifier(bool result)
        {
            _result = result;
        }

        public bool Verify(string callbackUrl)
        {
            return _result;
        }
    }
}
