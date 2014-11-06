using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NServiceBus;
using NServiceBus.Features;

namespace AccountCreateApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static IBus bus;

        private IStartableBus startableBus;

        public static IBus Bus
        {
            get { return bus; }
        }
        protected void Application_Start()
        {
            var config = new BusConfiguration();
            //config.TraceLogger();
            config.Conventions()
                .DefiningMessagesAs(
                    t =>
                        t.Namespace != null && t.Namespace.StartsWith("Messages") &&
                        t.Namespace.EndsWith("Messages"));
            config.UseTransport<MsmqTransport>();
            config.UsePersistence<InMemoryPersistence>();
            config.DisableFeature<TimeoutManager>();
            config.DisableFeature<SecondLevelRetries>();
            config.DisableFeature<Sagas>();
            config.PurgeOnStartup(false);
            startableBus = NServiceBus.Bus.Create(config);
            bus = startableBus.Start();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
