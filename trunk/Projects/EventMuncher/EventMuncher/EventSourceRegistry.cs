using System;

namespace EventMuncher
{
    public class EventSourceRegistry
    {
        public EventSourceRegistry(IEventSourcesConfigurationProvider configurationProvider)
        {
            configuration = configurationProvider.ProvideConfiguration();
        }

        public int EventSourcesCount
        {
            get { return configuration.EventSourcesCount; }
        }

        private EventSourcesConfiguration configuration;
    }
}