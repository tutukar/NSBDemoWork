
using System.Threading;
using NServiceBus.Features;
using NServiceBus.Persistence;
using NServiceBus.Persistence.Legacy;

namespace AccountCreateSubscriber
{
    using NServiceBus;

    /*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://particular.net/articles/the-nservicebus-host
	*/
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Client
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UsePersistence<MsmqPersistence>()
                .For(Storage.Subscriptions);
            configuration.UseTransport<MsmqTransport>();
            configuration.DisableFeature<TimeoutManager>();
        }
    }
}
