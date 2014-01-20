using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Windsor;
using UserService.Api.WindsorInstallers;

namespace UserService.Api
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        private readonly IWindsorContainer _container;

        public WebApiApplication()
        {
            _container = new WindsorContainer();
            LoadWindsorContainer(_container);
        }

        public static void LoadWindsorContainer(IWindsorContainer container)
        {
            container.Install(
                new ControllersInstaller(),
                new EventStoreInstaller(),
                new RepositoryInstaller());
        }

        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Services.Replace(
                typeof (IHttpControllerActivator),
                new WindsorCompositionRoot(_container));

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}