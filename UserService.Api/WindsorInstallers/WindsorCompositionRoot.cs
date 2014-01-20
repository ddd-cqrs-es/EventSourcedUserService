using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Castle.Windsor;

namespace UserService.Api.WindsorInstallers
{
    //taken from Mark Seemans blog here: http://blog.ploeh.dk/2012/10/03/DependencyInjectioninASP.NETWebAPIwithCastleWindsor/

    public class WindsorCompositionRoot : IHttpControllerActivator
    {
        private readonly IWindsorContainer _container;

        public WindsorCompositionRoot(IWindsorContainer container)
        {
            _container = container;
        }

        //creation of the controller is delegated to the Windsor container.
        //but, what we resolve, we must later release!
        //so, we register a "release" with the Request, so when the request is torn down, we release our controller
        //the Release is just a way to provide an IDisposable instance to the Request; when the Request disposes
        //the Release, the Release calls the closure to disposal of the controller instance from the Windsor container.
        public IHttpController Create(
            HttpRequestMessage request,
            HttpControllerDescriptor controllerDescriptor,
            Type controllerType)
        {
            var controller = (IHttpController)_container.Resolve(controllerType);

            request.RegisterForDispose(new Release(() => _container.Release(controller)));

            return controller;
        }

        //just a pattern that allows us to create a Disposable instance over a closure (the Action delegate) that includes
        //the Windsor container and the controller that we want to release (see above).
        //all that client code can see and get hold of is IDisposable, since this class is private;
        //clients can not get to the actual type.
        private class Release : IDisposable
        {
            private readonly Action _releaseDelegate;

            public Release(Action releaseDelegate)
            {
                _releaseDelegate = releaseDelegate;
            }

            public void Dispose()
            {
                _releaseDelegate();
            }
        }
    }
}