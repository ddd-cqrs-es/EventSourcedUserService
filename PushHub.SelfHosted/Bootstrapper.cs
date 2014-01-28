using System.IO;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace PushHub.SelfHosted
{
    using Nancy;

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));

            container.Register<ISubscriptionVerifier, SubscriptionVerifier>();
            container.Register<IFeedProvider, FeedProvider>();
            container.Register<Hub, Hub>().AsSingleton();
        }
    }
}