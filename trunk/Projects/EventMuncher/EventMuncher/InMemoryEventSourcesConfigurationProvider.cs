namespace EventMuncher
{
    public class InMemoryEventSourcesConfigurationProvider 
        : IEventSourcesConfigurationProvider
    {
        public int HowManySourcesToCreate
        {
            get { return howManySourcesToCreate; }
            set { howManySourcesToCreate = value; }
        }

        public EventSourcesConfiguration ProvideConfiguration()
        {
            EventSourcesConfiguration config = new EventSourcesConfiguration();

            for (int i = 0; i < howManySourcesToCreate; i++)
            {
                config.AddEventSource(new EventSource());
            }

            return config;
        }

        private int howManySourcesToCreate;
    }
}