
using NServiceBus.Features;
using NServiceBus.Persistence;
using NServiceBus.Persistence.Legacy;

namespace AccountCreateEndpoints
{
    using NServiceBus;

    /*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://particular.net/articles/the-nservicebus-host
	*/
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UsePersistence<MsmqPersistence>()
                .For(Storage.Subscriptions);
            configuration.DisableFeature<TimeoutManager>();
            /*configuration.UsePersistence<NHibernatePersistence>().
                For(Storage.Sagas).
                For(Storage.Timeouts);*/
            configuration.UseTransport<MsmqTransport>();

        }
    }
}
