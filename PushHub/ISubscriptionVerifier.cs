namespace PushHub
{
    public  interface ISubscriptionVerifier
    {
        bool Verify(string callbackUrl, string topic, string verifyToken);
    }
}